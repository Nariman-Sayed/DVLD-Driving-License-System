using System.Net;
using System.Net.Http.Json;
using DVLD.Shared;
using Xunit;

namespace DVLD.Api.IntegrationTests
{
    public class ApplicationTypeControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ApplicationTypeControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllTypes_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/ApplicationType");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var types = await response.Content.ReadFromJsonAsync<List<ApplicationTypeDto>>();
            Assert.NotNull(types);
            Assert.Equal(2, types.Count);
            Assert.Contains(types, t => t.ApplicationTypeID == 1 && t.ApplicationTypeTitle == "New Driving License Service");
            Assert.Contains(types, t => t.ApplicationTypeID == 2 && t.ApplicationTypeTitle == "Renew Driving License Service");
        }

        [Fact]
        public async Task GetTypeById_WithValidId_ReturnsType()
        {
            var response = await _client.GetAsync("/api/ApplicationType/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var type = await response.Content.ReadFromJsonAsync<ApplicationTypeDto>();
            Assert.NotNull(type);
            Assert.Equal(1, type.ApplicationTypeID);
            Assert.Equal("New Driving License Service", type.ApplicationTypeTitle);
            Assert.Equal(15m, type.ApplicationFees);
        }

        [Fact]
        public async Task GetTypeById_WithInvalidId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/ApplicationType/999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AddNewType_ReturnsCreatedWithLocation()
        {
            var newType = new ApplicationTypeDto
            {
                ApplicationTypeTitle = "Replacement for a Lost Driving License",
                ApplicationFees = 5m
            };

            var response = await _client.PostAsJsonAsync("/api/ApplicationType", newType);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
            Assert.EndsWith(
                "/api/ApplicationType/3",
                response.Headers.Location.ToString(),
                StringComparison.OrdinalIgnoreCase);

            var created = await response.Content.ReadFromJsonAsync<ApplicationTypeDto>();
            Assert.NotNull(created);
            Assert.Equal(3, created.ApplicationTypeID);
        }

        [Fact]
        public async Task GetAllTypes_WithoutToken_ReturnsUnauthorized()
        {
            // A fresh host without the TestAuth override, so the real JWT scheme
            // applies and the missing token yields 401 Unauthorized.
            using var factory = new UnauthenticatedWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/ApplicationType");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}