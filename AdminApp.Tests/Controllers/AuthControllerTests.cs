using System.Net;
using System.Net.Http.Json;
using AdminApp.Dtos;
using FluentAssertions;
using Xunit;

namespace AdminApp.Tests.Controllers
{
    public class AuthControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_ReturnsOkWithToken_ForValidCredentials()
        {
            var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto
            {
                Username = TestWebApplicationFactory.Username,
                Password = TestWebApplicationFactory.Password
            });

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            body.Should().NotBeNull();
            body!.Token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_ForInvalidPassword()
        {
            var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto
            {
                Username = TestWebApplicationFactory.Username,
                Password = "wrong-password"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_ForUnknownUsername()
        {
            var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto
            {
                Username = "not-admin",
                Password = TestWebApplicationFactory.Password
            });

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
