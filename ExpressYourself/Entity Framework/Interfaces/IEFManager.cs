using ExpressYourself.Entity_Framework.Types;

namespace ExpressYourself.Entity_Framework.Interfaces
{
    public interface IEFManager
    {
        Task<Countries> GetIpDetails(string IP);
        Task SetNewIP(Countries country, string IP);
        Task<List<SQLReport>> GetSqlReport();
        Task<List<IPToUpdate>> GetIPs(int rowsOffset);
        Task<bool> UpdateIPs(IPToUpdate row, string[] IP2CArray);
    }
}
