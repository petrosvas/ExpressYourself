using ExpressYourself.Extensions;
using ExpressYourself.Interfaces;
using ExpressYourself.Types;

namespace ExpressYourself.Adapters
{
    public class DBAdapter : IDBAdapter
    {
        private readonly IDBManager _manager;
        public DBAdapter(IDBManager manager)
        {
            _manager = manager;
        }

        public async Task<GenericResponse<CountryInfo>> GetCountryInfoByIP(string IP)
        {
            var info = await _manager.GetCountryInfoByIp(IP);
            return new GenericResponse<CountryInfo>
            {
                Found = info.Found,
                Value = info.Value.ToCountryInfo()
            };
        }

        public async Task<GenericResponse<CountryInfo>> GetCountryInfoUsingArrayString(string IP, string[] IP2CArray)
        {
            var info = await _manager.GetCountryInfoUsingArrayString(IP, IP2CArray);
            return new GenericResponse<CountryInfo>
            {
                Found = info.Found,
                Value = info.Value.ToCountryInfo()
            };
        }
    }
}
