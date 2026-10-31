using DVLD.Api.Data.Entities;
using DVLD.Api.Data.Interfaces;
using DVLD.Shared;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Data.Repositories
{
    internal sealed class BaseApplicationRepository(IApplicationDbContext context) : IBaseApplicationRepository
    {
        public async Task<List<BaseApplicationDto>> GetAllAsync()
        {
            return await context.Applications
                .AsNoTracking()
                .OrderByDescending(a => a.ApplicationDate)
                .Select(a => new BaseApplicationDto
                {
                    ApplicationID = a.ApplicationID,
                    ApplicantPersonID = a.ApplicantPersonID,
                    ApplicationDate = a.ApplicationDate,
                    ApplicationTypeID = a.ApplicationTypeID,
                    ApplicationStatus = a.ApplicationStatus,
                    LastStatusDate = a.LastStatusDate,
                    PaidFees = a.PaidFees,
                    CreatedByUserID = a.CreatedByUserID
                })
                .ToListAsync();
        }

        public async Task<BaseApplicationDto?> GetByIdAsync(int id)
        {
            return await context.Applications
                .AsNoTracking()
                .Where(a => a.ApplicationID == id)
                .Select(a => new BaseApplicationDto
                {
                    ApplicationID = a.ApplicationID,
                    ApplicantPersonID = a.ApplicantPersonID,
                    ApplicationDate = a.ApplicationDate,
                    ApplicationTypeID = a.ApplicationTypeID,
                    ApplicationStatus = a.ApplicationStatus,
                    LastStatusDate = a.LastStatusDate,
                    PaidFees = a.PaidFees,
                    CreatedByUserID = a.CreatedByUserID
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddAsync(BaseApplicationDto application)
        {
            var entity = new Application
            {
                ApplicantPersonID = application.ApplicantPersonID,
                ApplicationDate = application.ApplicationDate,
                ApplicationTypeID = application.ApplicationTypeID,
                ApplicationStatus = application.ApplicationStatus,
                LastStatusDate = application.LastStatusDate,
                PaidFees = application.PaidFees,
                CreatedByUserID = application.CreatedByUserID
            };

            context.Applications.Add(entity);
            await context.SaveChangesAsync();
            return entity.ApplicationID;
        }

        public async Task<bool> UpdateAsync(int id, BaseApplicationDto application)
        {
            var rowsAffected = await context.Applications
                .Where(a => a.ApplicationID == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.ApplicantPersonID, application.ApplicantPersonID)
                    .SetProperty(a => a.ApplicationDate, application.ApplicationDate)
                    .SetProperty(a => a.ApplicationTypeID, application.ApplicationTypeID)
                    .SetProperty(a => a.ApplicationStatus, application.ApplicationStatus)
                    .SetProperty(a => a.LastStatusDate, application.LastStatusDate)
                    .SetProperty(a => a.PaidFees, application.PaidFees)
                    .SetProperty(a => a.CreatedByUserID, application.CreatedByUserID));

            return rowsAffected > 0;
        }

        public async Task<int?> GetActiveApplicationIdAsync(int applicantPersonId, int applicationTypeId)
        {
            return await context.Applications
                .AsNoTracking()
                .Where(a => a.ApplicantPersonID == applicantPersonId
                    && a.ApplicationTypeID == applicationTypeId
                    && a.ApplicationStatus == 1)
                .Select(a => (int?)a.ApplicationID)
                .FirstOrDefaultAsync();
        }
    }
}
