using DVLD.Api.Data.Interfaces;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;
using Microsoft.Extensions.Logging;

namespace DVLD.Api.Services.Implementations
{
    internal sealed partial class PersonService(
        IPersonRepository repository,
        ILogger<PersonService> logger) : IPersonService
    {
        private readonly IPersonRepository _repository = repository;
        private readonly ILogger<PersonService> _logger = logger;

        public async Task<List<PersonDto>> GetAllPeopleAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PersonDto?> GetPersonByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<PersonDto> AddNewPersonAsync(PersonDto person)
        {
            LogAttemptingToAddPerson(person.NationalNo);
            person.PersonID = await _repository.AddAsync(person);
            LogSuccessfullyAddedPerson(person.PersonID);
            return person;
        }

        public async Task<bool> UpdatePersonAsync(int id, PersonDto person)
        {
            LogAttemptingToUpdatePerson(id);
            var updated = await _repository.UpdateAsync(id, person);

            if (!updated)
                LogPersonNotFoundForUpdate(id);
            else
                LogSuccessfullyUpdatedPerson(id);

            return updated;
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            LogAttemptingToDeletePerson(id);
            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
                LogPersonNotFoundForDelete(id);
            else
                LogSuccessfullyDeletedPerson(id);

            return deleted;
        }

        [LoggerMessage(Level = LogLevel.Information, Message = "Attempting to add new person with National No: {NationalNo}")]
        private partial void LogAttemptingToAddPerson(string nationalNo);

        [LoggerMessage(Level = LogLevel.Information, Message = "Successfully added person with ID: {PersonId}")]
        private partial void LogSuccessfullyAddedPerson(int personId);

        [LoggerMessage(Level = LogLevel.Information, Message = "Attempting to update person with ID: {PersonId}")]
        private partial void LogAttemptingToUpdatePerson(int personId);

        [LoggerMessage(Level = LogLevel.Warning, Message = "Update failed: Person with ID {PersonId} not found")]
        private partial void LogPersonNotFoundForUpdate(int personId);

        [LoggerMessage(Level = LogLevel.Information, Message = "Successfully updated person with ID: {PersonId}")]
        private partial void LogSuccessfullyUpdatedPerson(int personId);

        [LoggerMessage(Level = LogLevel.Information, Message = "Attempting to delete person with ID: {PersonId}")]
        private partial void LogAttemptingToDeletePerson(int personId);

        [LoggerMessage(Level = LogLevel.Warning, Message = "Delete failed: Person with ID {PersonId} not found")]
        private partial void LogPersonNotFoundForDelete(int personId);

        [LoggerMessage(Level = LogLevel.Information, Message = "Successfully deleted person with ID: {PersonId}")]
        private partial void LogSuccessfullyDeletedPerson(int personId);
    }
}
