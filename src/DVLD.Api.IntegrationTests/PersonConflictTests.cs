using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DVLD.Api.Constants;
using DVLD.Api.Data;
using Xunit;

namespace DVLD.Api.IntegrationTests
{
    public class PersonConflictTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PersonConflictTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddPerson_WithDuplicateNationalNo_ReturnsConflict()
        {
            var person = TestData.CreateValidPerson();
            person.NationalNo = "T1";

            var response = await _client.PostAsJsonAsync("/api/person", person);

            await AssertConflictAsync(response, ApiErrorMessages.DuplicateNationalNo);
        }

        [Fact]
        public async Task UpdatePerson_WithAnotherPersonsNationalNo_ReturnsConflict()
        {
            var person = TestData.CreateValidPerson();
            person.NationalNo = "T2";

            var response = await _client.PutAsJsonAsync("/api/person/1", person);

            await AssertConflictAsync(response, ApiErrorMessages.DuplicateNationalNo);
        }

        [Fact]
        public async Task UpdatePerson_WithOwnNationalNo_ReturnsNoContent()
        {
            var person = TestData.CreateValidPerson();
            person.NationalNo = "T1";

            var response = await _client.PutAsJsonAsync("/api/person/1", person);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeletePerson_ReferencedByOtherRecords_ReturnsConflict()
        {
            var response = await _client.DeleteAsync("/api/person/1");

            await AssertConflictAsync(response, ApiErrorMessages.PersonDeleteBlocked);
        }

        [Theory]
        [InlineData(DatabaseConflictTranslator.DuplicateKeyNumber, false)]
        [InlineData(DatabaseConflictTranslator.DuplicateConstraintNumber, false)]
        public void Translate_DuplicateKeyNumbers_ReturnsDuplicateConflict(int errorNumber, bool isDelete)
        {
            var conflict = DatabaseConflictTranslator.Translate(errorNumber, isDelete);

            Assert.NotNull(conflict);
            Assert.Equal(ApiErrorMessages.DuplicateNationalNo, conflict.Message);
        }

        [Fact]
        public void Translate_ForeignKeyOnDelete_ReturnsBlockedConflict()
        {
            var conflict = DatabaseConflictTranslator.Translate(
                DatabaseConflictTranslator.ForeignKeyViolationNumber,
                isDelete: true);

            Assert.NotNull(conflict);
            Assert.Equal(ApiErrorMessages.PersonDeleteBlocked, conflict.Message);
        }

        [Theory]
        [InlineData(1234, false)]
        [InlineData(547, false)]
        public void Translate_UnrelatedErrors_ReturnsNull(int errorNumber, bool isDelete)
        {
            Assert.Null(DatabaseConflictTranslator.Translate(errorNumber, isDelete));
        }

        private static async Task AssertConflictAsync(HttpResponseMessage response, string expectedDetail)
        {
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal(409, document.RootElement.GetProperty("status").GetInt32());
            Assert.Equal(ApiErrorMessages.ConflictTitle, document.RootElement.GetProperty("title").GetString());
            Assert.Equal(expectedDetail, document.RootElement.GetProperty("detail").GetString());
        }
    }
}
