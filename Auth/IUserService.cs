namespace Scraper.Auth
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
    }
}
