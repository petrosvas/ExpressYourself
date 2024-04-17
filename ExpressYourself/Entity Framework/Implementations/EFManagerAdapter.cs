using ExpressYourself.Entity_Framework.Extenstions;
using ExpressYourself.Entity_Framework.Interfaces;
using ExpressYourself.Entity_Framework.Types;
using ExpressYourself.Types;

namespace ExpressYourself.Entity_Framework.Implementations
{
    public class EFManagerAdapter : IEFManagerAdapter
    {
        IEFManager _manager;

        public EFManagerAdapter(IEFManager manager)
        {
            _manager = manager;
        }

        public async Task<GenericResponse<CountryInfo>> GetIpDetails(string IP)
        {
            var country = await _manager.GetIpDetails(IP);
            if (country != null) return new GenericResponse<CountryInfo>
            {
                Found = true,
                Value = country.ToCountryInfo()
            };
            return new GenericResponse<CountryInfo> { Found = false };
        }

        public async Task SetNewIP(CountryInfo countryInfo, string IP)
        {
            await _manager.SetNewIP(countryInfo.ToCountries(), IP);
        }

        public async Task<GenericResponse<List<SQLReport>>> GetSqlReport()
        {
            return new GenericResponse<List<SQLReport>> { Found = true, Value = await _manager.GetSqlReport() };
        }
    }
}
