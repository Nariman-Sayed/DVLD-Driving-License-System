using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    /// <summary>
    /// Persistence operations for <see cref="PersonDto"/> records.
    /// </summary>
    public interface IPersonRepository
    {
        /// <summary>
        /// Retrieves all people.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>All people; an empty list when none exist.</returns>
        Task<List<PersonDto>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single person by identifier.
        /// </summary>
        /// <param name="id">The person identifier.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The matching person, or <see langword="null"/> when no person exists.</returns>
        Task<PersonDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="person">The person to create.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The generated person identifier.</returns>
        Task<int> AddAsync(PersonDto person, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing person.
        /// </summary>
        /// <param name="id">The identifier of the person to update.</param>
        /// <param name="person">The new values for the person.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns><see langword="true"/> when the person was updated; otherwise <see langword="false"/>.</returns>
        Task<bool> UpdateAsync(int id, PersonDto person, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an existing person.
        /// </summary>
        /// <param name="id">The identifier of the person to delete.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns><see langword="true"/> when the person was deleted; otherwise <see langword="false"/>.</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
