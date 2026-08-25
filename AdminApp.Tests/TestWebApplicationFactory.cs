using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace AdminApp.Tests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        public const string Username = "admin";
        public const string Password = "ChangeMe123!";

        // PasswordHasher hash of "ChangeMe123!" used only for tests.
        private const string PasswordHash =
            "AQAAAAIAAYagAAAAEAiMaeJoLshcHOYC1xLB1h8muDqEHke1SUukyJePPpl7HPPSPqIgWTeloho1qEQ0Og==";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Auth:AdminUsername"] = Username,
                    ["Auth:AdminPasswordHash"] = PasswordHash,
                    ["Auth:JwtSecret"] = "test-only-secret-key-1234567890-abcdefgh",
                    ["Auth:JwtIssuer"] = "AdminApp.Tests",
                    ["Auth:JwtAudience"] = "AdminApp.Tests",
                    ["Auth:JwtExpiryMinutes"] = "60"
                });
            });
        }
    }
}
