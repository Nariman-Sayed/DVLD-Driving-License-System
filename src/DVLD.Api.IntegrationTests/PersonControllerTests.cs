using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
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
        public async Task GetPeople_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/person");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetPeople_ReturnsPagedResultWithDeterministicOrder()
        {
            var response = await _client.GetAsync("/api/person");
            var page = await response.Content.ReadFromJsonAsync<PagedResult<PersonDto>>();

            Assert.NotNull(page);
            Assert.Equal(1, page.PageNumber);
            Assert.Equal(20, page.PageSize);
            Assert.Equal(5, page.TotalCount);
            Assert.Equal(1, page.TotalPages);
            Assert.Equal(5, page.Items.Count);
            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, page.Items.Select(p => p.PersonID));
        }

        [Fact]
        public async Task GetPeople_WithPaging_ReturnsRequestedPage()
        {
            var response = await _client.GetAsync("/api/person?pageNumber=2&pageSize=2");
            var page = await response.Content.ReadFromJsonAsync<PagedResult<PersonDto>>();

            Assert.NotNull(page);
            Assert.Equal(2, page.PageNumber);
            Assert.Equal(2, page.PageSize);
            Assert.Equal(5, page.TotalCount);
            Assert.Equal(3, page.TotalPages);
            Assert.Equal(new[] { 3, 4 }, page.Items.Select(p => p.PersonID));
        }

        [Fact]
        public async Task GetPeople_WithNameFilter_ReturnsOnlyMatchingPeople()
        {
            var response = await _client.GetAsync("/api/person?filterOn=firstName&filterQuery=Alpha");
            var page = await response.Content.ReadFromJsonAsync<PagedResult<PersonDto>>();

            Assert.NotNull(page);
            Assert.Equal(1, page.TotalCount);
            Assert.Equal("Alpha", Assert.Single(page.Items).FirstName);
        }

        [Fact]
        public async Task GetPeople_WithCountryFilter_ReturnsOnlyMatchingPeople()
        {
            var response = await _client.GetAsync("/api/person?filterOn=countryId&filterQuery=2");
            var page = await response.Content.ReadFromJsonAsync<PagedResult<PersonDto>>();

            Assert.NotNull(page);
            Assert.Equal(2, page.TotalCount);
            Assert.Equal(new[] { 3, 4 }, page.Items.Select(p => p.PersonID));
        }

        [Fact]
        public async Task GetPeople_WithUnknownFilter_ReturnsBadRequest()
        {
            var response = await _client.GetAsync("/api/person?filterOn=phone&filterQuery=000");

            await AssertProblemDetailsAsync(response, HttpStatusCode.BadRequest, 400);
        }

        [Theory]
        [InlineData("pageNumber=0&pageSize=20")]
        [InlineData("pageNumber=1&pageSize=0")]
        [InlineData("pageNumber=1&pageSize=101")]
        public async Task GetPeople_WithInvalidPaging_ReturnsBadRequest(string queryString)
        {
            var response = await _client.GetAsync($"/api/person?{queryString}");

            await AssertProblemDetailsAsync(response, HttpStatusCode.BadRequest, 400);
        }

        [Fact]
        public async Task GetPeople_WithInvalidCountryFilter_ReturnsBadRequest()
        {
            var response = await _client.GetAsync("/api/person?filterOn=countryId&filterQuery=abc");

            await AssertProblemDetailsAsync(response, HttpStatusCode.BadRequest, 400);
        }

        [Fact]
        public async Task GetPeople_ReturnsOnlyDtoFields()
        {
            var response = await _client.GetAsync("/api/person?pageSize=1");
            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            var allowedFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "items", "pageNumber", "pageSize", "totalCount", "totalPages"
            };

            Assert.All(document.RootElement.EnumerateObject(), property =>
                Assert.Contains(property.Name, allowedFields));
        }

        [Fact]
        public async Task GetPeople_WithoutToken_ReturnsUnauthorized()
        {
            using var factory = new UnauthenticatedWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/person");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private static async Task AssertProblemDetailsAsync(
            HttpResponseMessage response,
            HttpStatusCode expectedStatus,
            int expectedProblemStatus)
        {
            Assert.Equal(expectedStatus, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal(expectedProblemStatus, document.RootElement.GetProperty("status").GetInt32());
        }
    }
}
