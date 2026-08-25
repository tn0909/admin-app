namespace AdminApp.Auth
{
    public interface IAuthService
    {
        bool ValidateCredentials(string username, string password);
        string GenerateToken(string username);
    }
}
