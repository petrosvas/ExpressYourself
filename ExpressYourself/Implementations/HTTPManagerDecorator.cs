using ExpressYourself.Exceptions;
using ExpressYourself.Extensions;
using ExpressYourself.Interfaces;
using ExpressYourself.Types;

namespace ExpressYourself.Implementations
{
    public class HTTPManagerDecorator : IHTTPManager
    {
        private readonly IHTTPManager _httpManager;

        public HTTPManagerDecorator(IHTTPManager httpManager)
        {
            _httpManager = httpManager;
        }
        public async Task<string[]> GetAsync(string URL)
        {
            var IP2CArray = await _httpManager.GetAsync(URL);
            return IP2CArray;
        }
    }
}
