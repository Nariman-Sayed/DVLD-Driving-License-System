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
        /// Retrieves one page of people with an optional allow-listed filter.
        /// </summary>
        /// <param name="query">The paging and filter values.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The requested page and the total number of matching people.</returns>
        /// <exception cref="DVLD.Api.Exceptions.ApplicationValidationException">Paging or filter values are invalid.</exception>
        Task<PagedResult<PersonDto>> GetPeopleAsync(PersonQuery query, CancellationToken cancellationToken = default);

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
