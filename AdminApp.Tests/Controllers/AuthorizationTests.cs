using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AdminApp.Dtos;
using FluentAssertions;
using Xunit;

namespace AdminApp.Tests.Controllers
{
    public class AuthorizationTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public AuthorizationTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Company_ReturnsUnauthorized_WithoutToken()
        {
            var response = await _client.GetAsync("/api/companies/some-id");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_User_ReturnsUnauthorized_WithoutToken()
        {
            var response = await _client.GetAsync("/api/users/some-id");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_Company_IsNotUnauthorized_WithValidToken()
        {
            var token = await GetTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/companies/some-id");

            response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        }

        private async Task<string> GetTokenAsync()
        {
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto
            {
                Username = TestWebApplicationFactory.Username,
                Password = TestWebApplicationFactory.Password
            });

            var body = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
            return body!.Token;
        }
    }
}
