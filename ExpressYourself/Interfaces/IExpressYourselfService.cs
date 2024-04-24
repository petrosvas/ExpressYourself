using ExpressYourself.Entity_Framework.Types;
using ExpressYourself.Types;

namespace ExpressYourself.Interfaces
{
    public interface IExpressYourselfService
    {
        Task<Response<CountryInfo>> GetIpDetailsEntityFramework(string IP);
        Task<Response<List<SQLReport>>> GetSqlReportEntityFramework();
        Task<Response<string>> UpdateIPsEntityFramework();
    }
}
