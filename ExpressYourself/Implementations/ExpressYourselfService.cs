using ExpressYourself.Adapters;
using ExpressYourself.Entity_Framework.DBContext;
using ExpressYourself.Entity_Framework.Interfaces;
using ExpressYourself.Entity_Framework.Types;
using ExpressYourself.Exceptions;
using ExpressYourself.Extensions;
using ExpressYourself.Interfaces;
using ExpressYourself.Types;

namespace ExpressYourself.Implementations
{
    public class ExpressYourselfService : IExpressYourselfService
    {
        private readonly ICachingManager _cachingManager;
        private readonly IDBAdapter _dbManagerAdapter;
        private readonly IDBManager _dbManager;
        private readonly IHTTPManager _httpManager;
        private readonly IEFManagerAdapter _efManagerAdapter;
        private readonly IEFManager _efManager;
        private readonly string baseUrl = "https://ip2c.org";
        AppDbContext _appDbContext;
        public ExpressYourselfService(ICachingManager cachingManager, IDBAdapter dbManagerAdapter, IHTTPManager hTTPManager, IDBManager dBManager,
            AppDbContext appDbContext, IEFManagerAdapter eFManagerAdapter, IEFManager eFManager)
        {
            _cachingManager = cachingManager;
            _dbManagerAdapter = dbManagerAdapter;
            _httpManager = hTTPManager;
            _dbManager = dBManager;
            _appDbContext = appDbContext;
            _efManagerAdapter = eFManagerAdapter;
            _efManager = eFManager;
        }

        public async Task<Response<CountryInfo>> GetIPDetails(string IP)
        {
            Validate(IP);
            GenericResponse<CountryInfo> countryInfo;

            countryInfo = _cachingManager.GetFromCache<CountryInfo>(IP);
            if (countryInfo.Found) return countryInfo.GetResponse();

            countryInfo = await _dbManagerAdapter.GetCountryInfoByIP(IP);
            if (countryInfo.Found)
            {
                _cachingManager.AddToCache(IP, countryInfo.Value);
                return countryInfo.GetResponse();
            }

            var IP2CArray = await _httpManager.GetAsync($"{baseUrl}/{IP}");
            _cachingManager.AddToCache(IP, IP2CArray.ToCountryInfo());
            countryInfo = await _dbManagerAdapter.GetCountryInfoUsingArrayString(IP, IP2CArray);
            return countryInfo.GetResponse();
        }

        private static void Validate(string IP)
        {
            string[] IPArray = IP.Split('.');
            if (IPArray.Length != 4) throw new InputException("Wrong IP Input: IP needs to be 4 bytes seperated by a dot. (e.g. 1.2.3.4)");
            IPArray.ToList().ForEach(b => { if (!byte.TryParse(b, out byte result)) throw new InputException($"Wrong IP Input: \"{b}\" is not a byte"); });
        }

        public async Task<Response<Sql_Report>> GetSqlReport()
        {
            GenericResponse<Sql_Report> countryInfo = await _dbManager.GetSqlTableReport();
            return countryInfo.GetResponse();
        }

        public async Task<Response<string>> UpdateIPs()
        {
            int rowsOffset = 0;
            bool hasRows;

            do
            {
                GenericResponse<Sql_Update> response = await _dbManager.GetIPsForUpdate(rowsOffset);
                Sql_Update sql_Update = response.Value;
                hasRows = response.Found;
                foreach (var row in sql_Update.Rows)
                {
                    string[] IP2CArray = await _httpManager.GetAsync($"{baseUrl}/{row.IP}");
                    await CheckUpdateIP(row, IP2CArray);
                }
                rowsOffset += 100;
            }
            while (hasRows);
            return new Response<string>
            {
                ResponseObject = "IP validation and updat completed successfully",
                ErrorCode = "",
                ErrorMessage = ""
            };
        }

        private async Task CheckUpdateIP(Sql_UpdateData row, string[] IP2CArray)
        {
            if (row.Name == IP2CArray.Name())
                return;

            await _dbManager.OpenConnection();
            int id;
            var CountryName = await _dbManager.GetCountryByName(IP2CArray.Name());
            if (!CountryName.Found)
            {
                id = await _dbManager.AddCountryToDB(IP2CArray.ToSql_Countries());
            }
            else
            {
                id = CountryName.Value.Id;
            }
            await _dbManager.ChangeCountryIdInIPAddresses(id, row.IP);
            _cachingManager.RemoveFromCache(row.IP);
            await _dbManager.CloseConnection();
        }

        public async Task<Response<CountryInfo>> GetIpDetailsEntityFramework(string IP)
        {
            Validate(IP);
            GenericResponse<CountryInfo> countryInfo;

            countryInfo = _cachingManager.GetFromCache<CountryInfo>(IP);
            if (countryInfo.Found) return countryInfo.GetResponse();

            countryInfo = await _efManagerAdapter.GetIpDetails(IP);
            if (countryInfo.Found)
            {
                _cachingManager.AddToCache(IP, countryInfo.Value);
                return countryInfo.GetResponse();
            }

            var IP2CArray = await _httpManager.GetAsync($"{baseUrl}/{IP}");
            _cachingManager.AddToCache(IP, IP2CArray.ToCountryInfo());
            await _efManagerAdapter.SetNewIP(IP2CArray.ToCountryInfo(), IP);
            return IP2CArray.ToCountryInfo().GetResponse();
        }

        public async Task<Response<List<SQLReport>>> GetSqlReportEntityFramework()
        {
            var response = await _efManagerAdapter.GetSqlReport();
            return response.GetResponse();
        }

        public async Task<Response<string>> UpdateIPsEntityFramework()
        {
            int rowsOffset = 0;
            bool hasRows;

            do
            {
                List<IPToUpdate> response = await _efManager.GetIPs(rowsOffset);
                response.ForEach(async row =>
                {
                    string[] IP2CArray = await _httpManager.GetAsync($"{baseUrl}/{row.IP}");
                    bool removeFromCache = await _efManager.UpdateIPs(row, IP2CArray);
                    if (removeFromCache) _cachingManager.RemoveFromCache(row.IP);
                });
                rowsOffset += 100;
                hasRows = response.Count > 0;
            }
            while (hasRows);
            return new Response<string>
            {
                ResponseObject = "IP validation and update completed successfully",
                ErrorCode = "",
                ErrorMessage = ""
            };
        }
    }
}
