using DVLD.Api.Constants;
using DVLD.Api.Data.Interfaces;
using DVLD.Api.Exceptions;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.Services.Implementations
{
    /// <summary>
    /// Application service implementation for <see cref="CountryDto"/> records.
    /// </summary>
    internal sealed class CountryService(
        ICountryRepository repository,
        ILogger<CountryService> logger) : ICountryService
    {
        private readonly ICountryRepository _repository = repository;
        private readonly ILogger<CountryService> _logger = logger;

        /// <inheritdoc />
        public async Task<List<CountryDto>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching all countries");
            return await _repository.GetAllAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<CountryDto> GetCountryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var country = await _repository.GetByIdAsync(id, cancellationToken);

            if (country is null)
            {
                _logger.LogInformation("Country with ID {CountryId} was not found", id);
                throw new NotFoundException(ApiErrorMessages.CountryNotFound);
            }

            return country;
        }
    }
}
