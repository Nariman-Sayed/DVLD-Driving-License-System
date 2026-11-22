using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    /// <summary>
    /// Persistence operations for <see cref="CountryDto"/> records.
    /// </summary>
    public interface ICountryRepository
    {
        /// <summary>
        /// Retrieves all countries ordered by name.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>All countries; an empty list when none exist.</returns>
        Task<List<CountryDto>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single country by identifier.
        /// </summary>
        /// <param name="id">The country identifier.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The matching country, or <see langword="null"/> when no country exists.</returns>
        Task<CountryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
