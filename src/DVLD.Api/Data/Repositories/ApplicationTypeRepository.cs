using DVLD.Api.Data.Entities;
using DVLD.Api.Data.Interfaces;
using DVLD.Shared;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Data.Repositories
{
    internal sealed class ApplicationTypeRepository(IApplicationDbContext context) : IApplicationTypeRepository
    {
        public async Task<List<ApplicationTypeDto>> GetAllAsync()
        {
            return await context.ApplicationTypes
                .AsNoTracking()
                .OrderBy(t => t.ApplicationTypeTitle)
                .Select(t => new ApplicationTypeDto
                {
                    ApplicationTypeID = t.ApplicationTypeID,
                    ApplicationTypeTitle = t.ApplicationTypeTitle,
                    ApplicationFees = t.ApplicationFees
                })
                .ToListAsync();
        }

        public async Task<ApplicationTypeDto?> GetByIdAsync(int id)
        {
            return await context.ApplicationTypes
                .AsNoTracking()
                .Where(t => t.ApplicationTypeID == id)
                .Select(t => new ApplicationTypeDto
                {
                    ApplicationTypeID = t.ApplicationTypeID,
                    ApplicationTypeTitle = t.ApplicationTypeTitle,
                    ApplicationFees = t.ApplicationFees
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddAsync(ApplicationTypeDto type)
        {
            var entity = new ApplicationType
            {
                ApplicationTypeTitle = type.ApplicationTypeTitle,
                ApplicationFees = type.ApplicationFees
            };

            context.ApplicationTypes.Add(entity);
            await context.SaveChangesAsync();
            return entity.ApplicationTypeID;
        }

        public async Task<bool> UpdateAsync(int id, ApplicationTypeDto type)
        {
            var rowsAffected = await context.ApplicationTypes
                .Where(t => t.ApplicationTypeID == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.ApplicationTypeTitle, type.ApplicationTypeTitle)
                    .SetProperty(t => t.ApplicationFees, type.ApplicationFees));

            return rowsAffected > 0;
        }
    }
}
