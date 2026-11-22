using System.Net;
using System.Net.Http.Json;
using DVLD.Shared;
using Xunit;

namespace DVLD.Api.IntegrationTests
{
    public class CountryControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CountryControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllCountries_ReturnsCountries()
        {
            var response = await _client.GetAsync("/api/Country");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var countries = await response.Content.ReadFromJsonAsync<List<CountryDto>>();
            Assert.NotNull(countries);
            Assert.Equal(3, countries.Count);
            Assert.Contains(countries, c => c.CountryID == 1 && c.CountryName == "United States");
        }

        [Fact]
        public async Task GetCountryById_WithValidId_ReturnsCountry()
        {
            var response = await _client.GetAsync("/api/Country/2");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var country = await response.Content.ReadFromJsonAsync<CountryDto>();
            Assert.NotNull(country);
            Assert.Equal(2, country.CountryID);
            Assert.Equal("United Kingdom", country.CountryName);
        }

        [Fact]
        public async Task GetCountryById_WithInvalidId_ReturnsNotFoundProblemDetails()
        {
            var response = await _client.GetAsync("/api/Country/999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

            var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
            Assert.NotNull(problem);
            Assert.Equal(404, problem.Status);
            Assert.Equal("Not found", problem.Title);
        }

        [Fact]
        public async Task GetAllCountries_WithoutToken_ReturnsUnauthorized()
        {
            using var factory = new UnauthenticatedWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/Country");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private sealed class ProblemDetailsResponse
        {
            public int Status { get; set; }
            public string Title { get; set; } = string.Empty;
        }
    }
}
