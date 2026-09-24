using DVLD.Api.Data.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.IntegrationTests.Fakes
{
    public class FakePersonRepository : IPersonRepository
    {
        private readonly List<PersonDto> _people = new()
        {
            new PersonDto
            {
                PersonID = 1,
                FirstName = "Test",
                SecondName = "User",
                ThirdName = "",
                LastName = "One",
                NationalNo = "T1",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gendor = 0,
                Address = "Test Address",
                Phone = "0000000",
                Email = "test@test.com",
                NationalityCountryID = 1,
                ImagePath = ""
            }
        };

        public Task<List<PersonDto>> GetAllAsync()
        {
            return Task.FromResult(_people.ToList());
        }

        public Task<PersonDto?> GetByIdAsync(int id)
        {
            var person = _people.FirstOrDefault(p => p.PersonID == id);
            return Task.FromResult(person);
        }

        public Task<int> AddAsync(PersonDto person)
        {
            person.PersonID = _people.Count > 0 ? _people.Max(p => p.PersonID) + 1 : 1;
            _people.Add(person);
            return Task.FromResult(person.PersonID);
        }

        public Task<bool> UpdateAsync(int id, PersonDto person)
        {
            var existing = _people.FirstOrDefault(p => p.PersonID == id);
            if (existing is null) return Task.FromResult(false);

            _people.Remove(existing);
            person.PersonID = id;
            _people.Add(person);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var existing = _people.FirstOrDefault(p => p.PersonID == id);
            if (existing is null) return Task.FromResult(false);

            _people.Remove(existing);
            return Task.FromResult(true);
        }
    }
}