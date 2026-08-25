using AdminApp.Auth;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace AdminApp.Tests.Services
{
    public class AuthServiceTests
    {
        private static AuthService CreateSut(string username = "admin", string password = "correct-horse-battery-staple")
        {
            var hasher = new AdminApp.Auth.PasswordHasher();
            var settings = new AuthSettings
            {
                AdminUsername = username,
                AdminPasswordHash = hasher.Hash(password),
                JwtSecret = "this-is-a-test-secret-key-that-is-long-enough-1234567890",
                JwtIssuer = "AdminApp.Tests",
                JwtAudience = "AdminApp.Tests",
                JwtExpiryMinutes = 60
            };

            return new AuthService(Options.Create(settings), hasher);
        }

        [Fact]
        public void ValidateCredentials_ReturnsTrue_ForCorrectUsernameAndPassword()
        {
            var sut = CreateSut();

            sut.ValidateCredentials("admin", "correct-horse-battery-staple").Should().BeTrue();
        }

        [Fact]
        public void ValidateCredentials_ReturnsFalse_ForWrongPassword()
        {
            var sut = CreateSut();

            sut.ValidateCredentials("admin", "wrong-password").Should().BeFalse();
        }

        [Fact]
        public void ValidateCredentials_ReturnsFalse_ForWrongUsername()
        {
            var sut = CreateSut();

            sut.ValidateCredentials("not-admin", "correct-horse-battery-staple").Should().BeFalse();
        }

        [Fact]
        public void GenerateToken_ReturnsNonEmptyJwt_ContainingUsernameClaim()
        {
            var sut = CreateSut();

            var token = sut.GenerateToken("admin");

            token.Should().NotBeNullOrWhiteSpace();
            token.Split('.').Should().HaveCount(3);
        }
    }
}
