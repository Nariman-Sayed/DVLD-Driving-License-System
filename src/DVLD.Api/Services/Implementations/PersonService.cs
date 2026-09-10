using DVLD.Api.Data.Interfaces;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.Services.Implementations
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;
        private readonly ILogger<PersonService> _logger;

        public PersonService(IPersonRepository repository, ILogger<PersonService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<PersonDto>> GetAllPeopleAsync()
        {
            _logger.LogInformation("Fetching all people");
            var people = await _repository.GetAllAsync();
            _logger.LogInformation("Successfully retrieved {Count} people", people.Count);
            return people;
        }

        public async Task<PersonDto?> GetPersonByIdAsync(int id)
        {
            _logger.LogInformation("Fetching person with ID: {PersonId}", id);
            var person = await _repository.GetByIdAsync(id);

            if (person is null)
                _logger.LogWarning("Person with ID {PersonId} not found", id);

            return person;
        }

        public async Task<PersonDto> AddNewPersonAsync(PersonDto person)
        {
            _logger.LogInformation("Attempting to add new person with National No: {NationalNo}", person.NationalNo);
            person.PersonID = await _repository.AddAsync(person);
            _logger.LogInformation("Successfully added person with ID: {PersonId}", person.PersonID);
            return person;
        }

        public async Task<bool> UpdatePersonAsync(int id, PersonDto person)
        {
            _logger.LogInformation("Attempting to update person with ID: {PersonId}", id);
            var updated = await _repository.UpdateAsync(id, person);

            if (!updated)
                _logger.LogWarning("Update failed: Person with ID {PersonId} not found", id);
            else
                _logger.LogInformation("Successfully updated person with ID: {PersonId}", id);

            return updated;
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            _logger.LogInformation("Attempting to delete person with ID: {PersonId}", id);
            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
                _logger.LogWarning("Delete failed: Person with ID {PersonId} not found", id);
            else
                _logger.LogInformation("Successfully deleted person with ID: {PersonId}", id);

            return deleted;
        }
    }
}