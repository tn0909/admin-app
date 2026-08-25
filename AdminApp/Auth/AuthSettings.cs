namespace AdminApp.Auth
{
    public class AuthSettings
    {
        public string AdminUsername { get; set; } = string.Empty;
        public string AdminPasswordHash { get; set; } = string.Empty;
        public string JwtSecret { get; set; } = string.Empty;
        public string JwtIssuer { get; set; } = string.Empty;
        public string JwtAudience { get; set; } = string.Empty;
        public int JwtExpiryMinutes { get; set; } = 60;
    }
}
