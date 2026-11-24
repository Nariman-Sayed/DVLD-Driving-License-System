using DVLD.Shared;

namespace DVLD.Api.Services.Interfaces
{
    /// <summary>
    /// Application service operations for <see cref="CountryDto"/> records.
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Retrieves all countries.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>All countries; an empty list when none exist.</returns>
        Task<List<CountryDto>> GetAllCountriesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single country by identifier.
        /// </summary>
        /// <param name="id">The country identifier.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The matching country.</returns>
        /// <exception cref="DVLD.Api.Exceptions.NotFoundException">No country exists with the given identifier.</exception>
        Task<CountryDto> GetCountryByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
