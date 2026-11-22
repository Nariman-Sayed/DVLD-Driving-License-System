using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DVLD.Api.Constants;
using DVLD.Api.IntegrationTests.Fakes;
using DVLD.Shared;
using Xunit;

namespace DVLD.Api.IntegrationTests
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_WithUnknownUser_ReturnsGenericUnauthorized()
        {
            await AssertGenericLoginFailureAsync("no-such-user", FakeUserRepository.ValidPassword);
        }

        [Fact]
        public async Task Login_WithWrongPassword_ReturnsGenericUnauthorized()
        {
            await AssertGenericLoginFailureAsync(FakeUserRepository.ActiveUsername, "wrong-password");
        }

        [Fact]
        public async Task Login_WithInactiveAccount_ReturnsGenericUnauthorized()
        {
            await AssertGenericLoginFailureAsync(FakeUserRepository.InactiveUsername, FakeUserRepository.ValidPassword);
        }

        [Fact]
        public async Task Login_WithPlaintextStoredPassword_ReturnsUnauthorizedNotServerError()
        {
            // The stored value is not a BCrypt hash. This used to make BCrypt.Verify throw
            // and the request return 500; it must now be an ordinary failed login.
            await AssertGenericLoginFailureAsync(
                FakeUserRepository.PlaintextUsername,
                FakeUserRepository.PlaintextPassword);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsToken()
        {
            var request = new LoginRequestDto
            {
                Username = FakeUserRepository.ActiveUsername,
                Password = FakeUserRepository.ValidPassword
            };

            var response = await _client.PostAsJsonAsync("/api/auth/login", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var login = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            Assert.NotNull(login);
            Assert.False(string.IsNullOrWhiteSpace(login.Token));
            Assert.Equal(1, login.UserID);
            Assert.Equal(FakeUserRepository.ActiveUsername, login.Username);
        }

        private async Task AssertGenericLoginFailureAsync(string username, string password)
        {
            var request = new LoginRequestDto
            {
                Username = username,
                Password = password
            };

            var response = await _client.PostAsJsonAsync("/api/auth/login", request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var message = document.RootElement.GetProperty("message").GetString();
            Assert.Equal(ApiErrorMessages.InvalidCredentials, message);
        }
    }
}
