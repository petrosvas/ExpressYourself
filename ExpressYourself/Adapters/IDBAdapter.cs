using ExpressYourself.Types;

namespace ExpressYourself.Adapters
{
    public interface IDBAdapter
    {
        Task<GenericResponse<CountryInfo>> GetCountryInfoByIP(string ip);
        Task<GenericResponse<CountryInfo>> GetCountryInfoUsingArrayString(string IP, string[] IP2CArray);
    }
}
