using ExpressYourself.Interfaces;
using ExpressYourself.Types;

namespace ExpressYourself.Implementations
{
    public class CachingManagerDecorator : ICachingManager
    {
        private readonly ICachingManager _cachingManager;

        public CachingManagerDecorator(ICachingManager cachingManager)
        {
            _cachingManager = cachingManager;
        }

        public GenericResponse<T> GetFromCache<T>(string IP)
        {
            var obj = _cachingManager.GetFromCache<T>(IP);
            obj.Found = obj.Value != null;
            return obj;
        }

        public bool ExistsInCache(string key)
        {
            return _cachingManager.ExistsInCache(key);
        }

        public void AddToCache(string key, object value, int minutesOffset = 60)
        {
            if (!ExistsInCache(key)) _cachingManager.AddToCache(key, value, minutesOffset);
        }

        public void RemoveFromCache(string key)
        {
            if (ExistsInCache(key)) _cachingManager.RemoveFromCache(key);
        }
    }
}
