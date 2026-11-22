using DVLD.Shared;

namespace DVLD.Api.IntegrationTests
{
    /// <summary>
    /// Shared valid person values for request body tests.
    /// </summary>
    internal static class TestData
    {
        public static PersonDto CreateValidPerson()
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
