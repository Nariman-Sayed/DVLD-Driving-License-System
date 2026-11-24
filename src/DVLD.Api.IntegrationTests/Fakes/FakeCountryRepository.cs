using DVLD.Api.Data.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.IntegrationTests.Fakes
{
    /// <summary>
    /// In-memory country repository used by integration tests so CI needs no database.
    /// </summary>
    public sealed class FakeCountryRepository : ICountryRepository
    {
        private readonly List<CountryDto> _countries = new()
        {
            new CountryDto { CountryID = 1, CountryName = "United States" },
            new CountryDto { CountryID = 2, CountryName = "United Kingdom" },
            new CountryDto { CountryID = 3, CountryName = "Canada" }
        };

        /// <inheritdoc />
        public Task<List<CountryDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_countries.OrderBy(c => c.CountryName).ToList());
        }

        /// <inheritdoc />
        public Task<CountryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_countries.FirstOrDefault(c => c.CountryID == id));
        }
    }
}
