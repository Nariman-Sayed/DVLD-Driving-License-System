using System.Net;
using System.Net.Http.Json;
using DVLD.Shared;
using Xunit;

namespace DVLD.Api.IntegrationTests
{
    public class PersonControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PersonControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllPeople_ReturnsSuccessStatusCode()
        {
            // The fake repository is already injected and seeded with one person,
            // so no extra arrangement is needed.

            var response = await _client.GetAsync("/api/person");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllPeople_ReturnsExpectedPerson()
        {
            var response = await _client.GetAsync("/api/person");
            var people = await response.Content.ReadFromJsonAsync<List<PersonDto>>();

            Assert.NotNull(people);
            var person = Assert.Single(people);
            Assert.Equal("T1", person.NationalNo);
            Assert.Equal("Test", person.FirstName);
            Assert.Equal("One", person.LastName);
        }

        [Fact]
        public async Task GetAllPeople_WithoutToken_ReturnsUnauthorized()
        {
            // A fresh host without the TestAuth override, so the real JWT scheme
            // applies and the missing token yields 401 Unauthorized.
            using var factory = new UnauthenticatedWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/person");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}