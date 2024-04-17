namespace ExpressYourself.Interfaces
{
    public interface IHTTPManager
    {
        Task<string[]> GetAsync(string URL);
    }
}
