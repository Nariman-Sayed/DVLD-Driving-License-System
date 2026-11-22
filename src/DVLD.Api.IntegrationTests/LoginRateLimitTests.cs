using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DVLD.Shared;
using Xunit;

namespace DVLD.Api.IntegrationTests
{
    public class LoginRateLimitTests : IClassFixture<RateLimitedWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public LoginRateLimitTests(RateLimitedWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_WhenLimitExceeded_Returns429ProblemDetails()
        {
            var request = new LoginRequestDto
            {
                Username = "no-such-user",
                Password = "irrelevant"
            };

            // The dedicated policy allows two requests per window in this host.
            for (var attempt = 0; attempt < 2; attempt++)
            {
                using var allowed = await _client.PostAsJsonAsync("/api/auth/login", request);
                Assert.NotEqual(HttpStatusCode.TooManyRequests, allowed.StatusCode);
            }

            using var limited = await _client.PostAsJsonAsync("/api/auth/login", request);

            Assert.Equal(HttpStatusCode.TooManyRequests, limited.StatusCode);
            Assert.Equal("application/problem+json", limited.Content.Headers.ContentType?.MediaType);

            using var document = JsonDocument.Parse(await limited.Content.ReadAsStringAsync());
            Assert.Equal(429, document.RootElement.GetProperty("status").GetInt32());
            Assert.Equal("Too many requests", document.RootElement.GetProperty("title").GetString());
            Assert.Contains(
                "Request rate limit exceeded",
                document.RootElement.GetProperty("detail").GetString());
        }
    }
}
