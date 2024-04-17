using ExpressYourself.Interfaces;

namespace ExpressYourself.Implementations
{
    public class HTTPManager : IHTTPManager
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<string[]> GetAsync(string URL)
        {
            var str = await _httpClient.GetAsync(new Uri(URL)).Result.Content.ReadAsStringAsync();
            return str.Split(';');
        }
    }
}
