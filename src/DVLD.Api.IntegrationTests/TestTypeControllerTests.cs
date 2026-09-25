using System.Net;
using System.Net.Http.Json;
using DVLD.Shared;
using Xunit;

namespace DVLD.Api.IntegrationTests
{
    public class TestTypeControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TestTypeControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllTestTypes_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/TestType");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var types = await response.Content.ReadFromJsonAsync<List<TestTypeDto>>();
            Assert.NotNull(types);
            Assert.Equal(3, types.Count);
            Assert.Contains(types, t => t.TestTypeID == 1 && t.TestTypeTitle == "Vision Test");
            Assert.Contains(types, t => t.TestTypeID == 2 && t.TestTypeTitle == "Written Test");
            Assert.Contains(types, t => t.TestTypeID == 3 && t.TestTypeTitle == "Street Test");
        }

        [Fact]
        public async Task GetTestTypeById_WithValidId_ReturnsType()
        {
            var response = await _client.GetAsync("/api/TestType/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var type = await response.Content.ReadFromJsonAsync<TestTypeDto>();
            Assert.NotNull(type);
            Assert.Equal(1, type.TestTypeID);
            Assert.Equal("Vision Test", type.TestTypeTitle);
            Assert.Equal(10m, type.TestTypeFees);
        }

        [Fact]
        public async Task GetTestTypeById_WithInvalidId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/TestType/999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AddNewTestType_ReturnsCreatedWithLocation()
        {
            var newType = new TestTypeDto
            {
                TestTypeTitle = "Special Medical Test",
                TestTypeDescription = "Additional medical assessment.",
                TestTypeFees = 15m
            };

            var response = await _client.PostAsJsonAsync("/api/TestType", newType);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
            Assert.EndsWith(
                "/api/TestType/4",
                response.Headers.Location.ToString(),
                StringComparison.OrdinalIgnoreCase);

            var created = await response.Content.ReadFromJsonAsync<TestTypeDto>();
            Assert.NotNull(created);
            Assert.Equal(4, created.TestTypeID);
        }

        [Fact]
        public async Task GetAllTestTypes_WithoutToken_ReturnsUnauthorized()
        {
            // A fresh host without the TestAuth override, so the real JWT scheme
            // applies and the missing token yields 401 Unauthorized.
            using var factory = new UnauthenticatedWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/TestType");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}