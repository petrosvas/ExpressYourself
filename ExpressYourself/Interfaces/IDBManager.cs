using ExpressYourself.Types;

namespace ExpressYourself.Interfaces
{
    public interface IDBManager
    {
        Task<GenericResponse<Sql_Countries>> GetCountryInfoByIp(string IP);
        Task<GenericResponse<Sql_Countries>> GetCountryInfoUsingArrayString(string IP, string[] IP2CArray);
        Task<GenericResponse<Sql_Report>> GetSqlTableReport();
        Task<GenericResponse<Sql_Update>> GetIPsForUpdate(int rowsOffset);
        Task<GenericResponse<Sql_Countries>> GetCountryByName(string name);
        Task<int> AddCountryToDB(Sql_Countries sql_Countries);
        Task ChangeCountryIdInIPAddresses(int id, string IP);
        Task OpenConnection();
        Task CloseConnection();
    }
}
