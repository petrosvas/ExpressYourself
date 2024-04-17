using ExpressYourself.Exceptions;
using ExpressYourself.Extensions;
using ExpressYourself.Interfaces;
using ExpressYourself.Types;

namespace ExpressYourself.Implementations
{
    public class HTTPManagerDecorator : IHTTPManager
    {
        private static readonly List<string> errorStrings = new List<string>
        {
            "XW","XX","XY","XZ","ZZ"
        };
        private readonly IHTTPManager _httpManager;

        public HTTPManagerDecorator(IHTTPManager httpManager)
        {
            _httpManager = httpManager;
        }
        public async Task<string[]> GetAsync(string URL)
        {
            var IP2CArray = await _httpManager.GetAsync(URL);
            Validate(IP2CArray);
            return IP2CArray;
        }

        private static void Validate(string[] IP2CArray)
        {
            if (IP2CArray.Code() == "0") throw new HttpException("The IP Provided has wrong input!") { Severity = SeverityCodes.User_error };
            if (IP2CArray.Code() == "2") throw new HttpException("www.IP2C.org has returned error \"Unknown\"!");
            if (IP2CArray.Code() == "1")
            {
                if (errorStrings.Contains(IP2CArray.TwoLetterCode())) throw new HttpException($"Non-geographical IP returned. Error returned by www.IP2C.org: {IP2CArray.Name()}");
            }
            else
                throw new HttpException("Unexpected response from www.IP2C.org!");
            if (IP2CArray.Length != 4) throw new HttpException("String array length taken from www.IP2C.org is not 4!");
        }
    }
}
