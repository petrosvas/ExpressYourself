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
        private readonly IHTTPManager _httpManager;
        private readonly IEFManagerAdapter _efManagerAdapter;
        private readonly IEFManager _efManager;
        private readonly string baseUrl = "https://ip2c.org";
        public ExpressYourselfService(ICachingManager cachingManager, IHTTPManager hTTPManager,
            IEFManagerAdapter eFManagerAdapter, IEFManager eFManager)
        {
            _cachingManager = cachingManager;
            _httpManager = hTTPManager;
            _efManagerAdapter = eFManagerAdapter;
            _efManager = eFManager;
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

        private static void Validate(string IP)
        {
            string[] IPArray = IP.Split('.');
            if (IPArray.Length != 4) throw new InputException("Wrong IP Input: IP needs to be 4 bytes seperated by a dot. (e.g. 1.2.3.4)");
            IPArray.ToList().ForEach(b => { if (!byte.TryParse(b, out byte result)) throw new InputException($"Wrong IP Input: \"{b}\" is not a byte"); });
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
