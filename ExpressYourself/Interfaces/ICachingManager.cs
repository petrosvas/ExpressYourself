using ExpressYourself.Types;

namespace ExpressYourself.Interfaces
{
    public interface ICachingManager
    {
        GenericResponse<T> GetFromCache<T>(string IP);
        bool ExistsInCache(string key);
        void AddToCache(string key, object value, int minutesOffset = 60);
        void RemoveFromCache(string key);
    }
}
