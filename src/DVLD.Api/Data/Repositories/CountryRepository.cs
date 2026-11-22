using DVLD.Api.Data.Interfaces;
using DVLD.Shared;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Data.Repositories
{
    internal sealed class CountryRepository(IApplicationDbContext context) : ICountryRepository
    {
        public async Task<List<CountryDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Countries
                .AsNoTracking()
                .OrderBy(c => c.CountryName)
                .Select(c => new CountryDto
                {
                    CountryID = c.CountryID,
                    CountryName = c.CountryName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<CountryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Countries
                .AsNoTracking()
                .Where(c => c.CountryID == id)
                .Select(c => new CountryDto
                {
                    CountryID = c.CountryID,
                    CountryName = c.CountryName
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
