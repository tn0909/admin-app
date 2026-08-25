using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AdminApp.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AuthSettings _settings;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IOptions<AuthSettings> settings, IPasswordHasher passwordHasher)
        {
            _settings = settings.Value;
            _passwordHasher = passwordHasher;
        }

        public bool ValidateCredentials(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (!string.Equals(username, _settings.AdminUsername, StringComparison.Ordinal))
            {
                return false;
            }

            return _passwordHasher.Verify(_settings.AdminPasswordHash, password);
        }

        public string GenerateToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _settings.JwtIssuer,
                audience: _settings.JwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.JwtExpiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
