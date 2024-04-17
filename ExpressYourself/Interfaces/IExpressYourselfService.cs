using ExpressYourself.Entity_Framework.Types;
using ExpressYourself.Types;

namespace ExpressYourself.Interfaces
{
    public interface IExpressYourselfService
    {
        Task<Response<CountryInfo>> GetIPDetails(string IP);
        Task<Response<Sql_Report>> GetSqlReport();
        Task<Response<string>> UpdateIPs();
        Task<Response<CountryInfo>> GetIpDetailsEntityFramework(string IP);
        Task<Response<List<SQLReport>>> GetSqlReportEntityFramework();
        Task<Response<string>> UpdateIPsEntityFramework();
    }
}
