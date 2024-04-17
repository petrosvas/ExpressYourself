using ExpressYourself.Entity_Framework.Types;
using ExpressYourself.Types;

namespace ExpressYourself.Entity_Framework.Interfaces
{
    public interface IEFManagerAdapter
    {
        Task<GenericResponse<CountryInfo>> GetIpDetails(string IP);
        Task SetNewIP(CountryInfo countryInfo, string IP);
        Task<GenericResponse<List<SQLReport>>> GetSqlReport();
    }
}
