using DVLD.Api.Data.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.IntegrationTests.Fakes
{
    public class FakeBaseApplicationRepository : IBaseApplicationRepository
    {
        private readonly List<BaseApplicationDto> _applications = new()
        {
            new BaseApplicationDto
            {
                ApplicationID = 1,
                ApplicantPersonID = 1,
                ApplicationDate = new DateTime(2026, 1, 1),
                ApplicationTypeID = 1,
                ApplicationStatus = 1,
                LastStatusDate = new DateTime(2026, 1, 1),
                PaidFees = 15,
                CreatedByUserID = 1
            }
        };

        public Task<List<BaseApplicationDto>> GetAllAsync()
        {
            return Task.FromResult(_applications.OrderByDescending(a => a.ApplicationDate).ToList());
        }

        public Task<BaseApplicationDto?> GetByIdAsync(int id)
        {
            var application = _applications.FirstOrDefault(a => a.ApplicationID == id);
            return Task.FromResult(application);
        }

        public Task<int> AddAsync(BaseApplicationDto application)
        {
            application.ApplicationID = _applications.Count > 0
                ? _applications.Max(a => a.ApplicationID) + 1
                : 1;
            _applications.Add(application);
            return Task.FromResult(application.ApplicationID);
        }

        public Task<bool> UpdateAsync(int id, BaseApplicationDto application)
        {
            var existing = _applications.FirstOrDefault(a => a.ApplicationID == id);
            if (existing is null) return Task.FromResult(false);

            _applications.Remove(existing);
            application.ApplicationID = id;
            _applications.Add(application);
            return Task.FromResult(true);
        }

        public Task<int?> GetActiveApplicationIdAsync(int applicantPersonId, int applicationTypeId)
        {
            var applicationId = _applications.FirstOrDefault(
                a => a.ApplicantPersonID == applicantPersonId
                     && a.ApplicationTypeID == applicationTypeId
                     && a.ApplicationStatus == 1)?.ApplicationID;
            return Task.FromResult(applicationId);
        }
    }
}