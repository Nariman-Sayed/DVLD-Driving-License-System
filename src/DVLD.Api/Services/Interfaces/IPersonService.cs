using DVLD.Shared;

namespace DVLD.Api.Services.Interfaces
{
    /// <summary>
    /// Application-level operations for managing <see cref="PersonDto"/> records.
    /// </summary>
    // Public because PersonController is public (MVC discovery) and its constructor
    // parameter types must be at least as accessible as the constructor (CS0051).
    public interface IPersonService
    {
        /// <summary>
        /// Retrieves all people.
        /// </summary>
        /// <returns>A read-only result containing every person; an empty list when there are none.</returns>
        Task<List<PersonDto>> GetAllPeopleAsync();

        /// <summary>
        /// Retrieves a single person by identifier.
        /// </summary>
        /// <param name="id">The person identifier.</param>
        /// <returns>The matching person, or <see langword="null"/> when no person exists with the given identifier.</returns>
        Task<PersonDto?> GetPersonByIdAsync(int id);

        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="person">The person to create. The generated identifier is assigned to the returned value.</param>
        /// <returns>The created person including its generated <see cref="PersonDto.PersonID"/>.</returns>
        Task<PersonDto> AddNewPersonAsync(PersonDto person);

        /// <summary>
        /// Updates an existing person.
        /// </summary>
        /// <param name="id">The identifier of the person to update.</param>
        /// <param name="person">The new values for the person.</param>
        /// <returns><see langword="true"/> when the person was updated; otherwise <see langword="false"/>.</returns>
        Task<bool> UpdatePersonAsync(int id, PersonDto person);

        /// <summary>
        /// Deletes an existing person.
        /// </summary>
        /// <param name="id">The identifier of the person to delete.</param>
        /// <returns><see langword="true"/> when the person was deleted; otherwise <see langword="false"/>.</returns>
        Task<bool> DeletePersonAsync(int id);
    }
}
