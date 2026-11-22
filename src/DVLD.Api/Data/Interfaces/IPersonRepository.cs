using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    /// <summary>
    /// Persistence operations for <see cref="PersonDto"/> records.
    /// </summary>
    public interface IPersonRepository
    {
        /// <summary>
        /// Retrieves one page of people with an optional allow-listed filter.
        /// </summary>
        /// <param name="query">The paging and filter values.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The requested page and the total number of matching people.</returns>
        Task<PagedResult<PersonDto>> GetPagedAsync(PersonQuery query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single person by identifier.
        /// </summary>
        /// <param name="id">The person identifier.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The matching person, or <see langword="null"/> when no person exists.</returns>
        Task<PersonDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether a national number is already used by another person.
        /// </summary>
        /// <param name="nationalNo">The national number to check.</param>
        /// <param name="excludePersonId">An optional person identifier to exclude from the check.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns><see langword="true"/> when the value is already in use.</returns>
        Task<bool> NationalNoExistsAsync(
            string nationalNo,
            int? excludePersonId = null,
            CancellationToken cancellationToken = default);

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

        /// <summary>
        /// Retrieves the stored image file name for a person.
        /// </summary>
        /// <param name="id">The person identifier.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The stored value, or <see langword="null"/> when the person does not exist.</returns>
        Task<string?> GetImagePathAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Stores an image file name for a person.
        /// </summary>
        /// <param name="id">The person identifier.</param>
        /// <param name="fileName">The generated file name, without any directory path.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns><see langword="true"/> when the person was updated; otherwise <see langword="false"/>.</returns>
        Task<bool> SetImagePathAsync(int id, string fileName, CancellationToken cancellationToken = default);
    }
}
