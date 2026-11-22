using System.Net;
using System.Net.Http.Json;
using DVLD.Shared;
using Xunit;

namespace DVLD.Api.IntegrationTests
{
    public class PersonValidationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PersonValidationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddPerson_WithValidBody_ReturnsCreated()
        {
            var response = await _client.PostAsJsonAsync("/api/person", CreateValidPerson());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task AddPerson_WithMissingRequiredFields_ReturnsBadRequest()
        {
            var person = CreateValidPerson();
            person.NationalNo = string.Empty;
            person.FirstName = string.Empty;
            person.Address = string.Empty;

            var response = await _client.PostAsJsonAsync("/api/person", person);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        }

        [Theory]
        [InlineData(nameof(PersonDto.FirstName), 21)]
        [InlineData(nameof(PersonDto.LastName), 21)]
        [InlineData(nameof(PersonDto.NationalNo), 21)]
        [InlineData(nameof(PersonDto.Phone), 21)]
        [InlineData(nameof(PersonDto.Email), 51)]
        [InlineData(nameof(PersonDto.Address), 501)]
        public async Task AddPerson_WithTooLongField_ReturnsBadRequest(string propertyName, int length)
        {
            var person = CreateValidPerson();
            var value = new string('x', length);

            switch (propertyName)
            {
                case nameof(PersonDto.FirstName):
                    person.FirstName = value;
                    break;
                case nameof(PersonDto.LastName):
                    person.LastName = value;
                    break;
                case nameof(PersonDto.NationalNo):
                    person.NationalNo = value;
                    break;
                case nameof(PersonDto.Phone):
                    person.Phone = value;
                    break;
                case nameof(PersonDto.Email):
                    person.Email = value;
                    break;
                default:
                    person.Address = value;
                    break;
            }

            var response = await _client.PostAsJsonAsync("/api/person", person);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePerson_WithInvalidBody_ReturnsBadRequest()
        {
            var person = CreateValidPerson();
            person.NationalityCountryID = 0;

            var response = await _client.PutAsJsonAsync("/api/person/1", person);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private static PersonDto CreateValidPerson()
        {
            return new PersonDto
            {
                FirstName = "Valid",
                SecondName = "Test",
                ThirdName = string.Empty,
                LastName = "Person",
                NationalNo = "V1",
                DateOfBirth = new DateTime(1995, 5, 20),
                Gendor = 0,
                Address = "Valid Address",
                Phone = "1234567",
                Email = "valid@example.com",
                NationalityCountryID = 1,
                ImagePath = string.Empty
            };
        }
    }
}
