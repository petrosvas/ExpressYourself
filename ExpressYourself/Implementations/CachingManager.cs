using ExpressYourself.Interfaces;
using ExpressYourself.Types;
using System.Runtime.Caching;

namespace ExpressYourself.Implementations
{
    public class CachingManager : ICachingManager
    {
        private static readonly MemoryCache _memoryCache = MemoryCache.Default;

        public GenericResponse<T> GetFromCache<T>(string IP)
        {
            return new GenericResponse<T>
            {
                Value = (T)_memoryCache.Get(IP)
            };
        }

        public bool ExistsInCache(string key)
        {
            return _memoryCache.Contains(key);
        }

        public void AddToCache(string key, object value, int minutesOffset = 60)
        {
            _memoryCache.Add(key, value, DateTimeOffset.UtcNow.AddMinutes(minutesOffset));
        }

        public void RemoveFromCache(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
