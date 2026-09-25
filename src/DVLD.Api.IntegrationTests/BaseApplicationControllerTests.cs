using System.Net;
using System.Net.Http.Json;
using DVLD.Shared;
using Xunit;

namespace DVLD.Api.IntegrationTests
{
    public class BaseApplicationControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public BaseApplicationControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllApplications_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/BaseApplication");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var applications = await response.Content.ReadFromJsonAsync<List<BaseApplicationDto>>();
            Assert.NotNull(applications);
            Assert.Contains(applications, a => a.ApplicationID == 1 && a.ApplicantPersonID == 1);
        }

        [Fact]
        public async Task GetApplicationById_WithValidId_ReturnsApplication()
        {
            var response = await _client.GetAsync("/api/BaseApplication/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var application = await response.Content.ReadFromJsonAsync<BaseApplicationDto>();
            Assert.NotNull(application);
            Assert.Equal(1, application.ApplicationID);
            Assert.Equal(1, application.ApplicantPersonID);
            Assert.Equal(1, application.ApplicationTypeID);
            Assert.Equal(1, application.ApplicationStatus);
            Assert.Equal(15m, application.PaidFees);
        }

        [Fact]
        public async Task GetApplicationById_WithInvalidId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/BaseApplication/999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AddNewApplication_ReturnsCreatedWithLocation()
        {
            var newApplication = new CreateBaseApplicationRequest
            {
                ApplicantPersonID = 1,
                ApplicationTypeID = 2
            };

            var response = await _client.PostAsJsonAsync("/api/BaseApplication", newApplication);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
            Assert.EndsWith(
                "/api/BaseApplication/2",
                response.Headers.Location.ToString(),
                StringComparison.OrdinalIgnoreCase);

            var created = await response.Content.ReadFromJsonAsync<BaseApplicationDto>();
            Assert.NotNull(created);
            Assert.Equal(2, created.ApplicationID);
            Assert.Equal(1, created.ApplicantPersonID);
            Assert.Equal(2, created.ApplicationTypeID);

            // Server-derived fields: the client only sent person + type.
            Assert.Equal(1, created.ApplicationStatus);
            Assert.Equal(1, created.CreatedByUserID);
            Assert.NotEqual(default, created.ApplicationDate);
            Assert.NotEqual(default, created.LastStatusDate);
        }

        [Fact]
        public async Task GetAllApplications_WithoutToken_ReturnsUnauthorized()
        {
            // A fresh host without the TestAuth override, so the real JWT scheme
            // applies and the missing token yields 401 Unauthorized.
            using var factory = new UnauthenticatedWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/BaseApplication");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}