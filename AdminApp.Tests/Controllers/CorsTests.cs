using FluentAssertions;
using Xunit;

namespace AdminApp.Tests.Controllers
{
    public class CorsTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CorsTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Preflight_DoesNotAllowCredentials()
        {
            var request = new HttpRequestMessage(HttpMethod.Options, "/api/companies");
            request.Headers.Add("Origin", "http://localhost:4200");
            request.Headers.Add("Access-Control-Request-Method", "POST");

            var response = await _client.SendAsync(request);

            response.Headers.Contains("Access-Control-Allow-Credentials").Should().BeFalse();
        }

        [Fact]
        public async Task Preflight_DoesNotAllowArbitraryHeaders()
        {
            var request = new HttpRequestMessage(HttpMethod.Options, "/api/companies");
            request.Headers.Add("Origin", "http://localhost:4200");
            request.Headers.Add("Access-Control-Request-Method", "POST");
            request.Headers.Add("Access-Control-Request-Headers", "X-Totally-Made-Up-Header");

            var response = await _client.SendAsync(request);

            response.Headers.TryGetValues("Access-Control-Allow-Headers", out var allowedHeaders).Should().BeTrue();
            string.Join(",", allowedHeaders!).Should().NotContain("X-Totally-Made-Up-Header");
        }

        [Fact]
        public async Task Preflight_RejectsDisallowedOrigin()
        {
            var request = new HttpRequestMessage(HttpMethod.Options, "/api/companies");
            request.Headers.Add("Origin", "http://evil.example.com");
            request.Headers.Add("Access-Control-Request-Method", "POST");

            var response = await _client.SendAsync(request);

            response.Headers.Contains("Access-Control-Allow-Origin").Should().BeFalse();
        }
    }
}
