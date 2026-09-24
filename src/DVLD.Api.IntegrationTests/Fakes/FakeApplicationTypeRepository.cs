using DVLD.Api.Data.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.IntegrationTests.Fakes
{
    public class FakeApplicationTypeRepository : IApplicationTypeRepository
    {
        private readonly List<ApplicationTypeDto> _types = new()
        {
            new ApplicationTypeDto
            {
                ApplicationTypeID = 1,
                ApplicationTypeTitle = "New Driving License Service",
                ApplicationFees = 15
            },
            new ApplicationTypeDto
            {
                ApplicationTypeID = 2,
                ApplicationTypeTitle = "Renew Driving License Service",
                ApplicationFees = 5
            }
        };

        public Task<List<ApplicationTypeDto>> GetAllAsync()
        {
            return Task.FromResult(_types.OrderBy(t => t.ApplicationTypeTitle).ToList());
        }

        public Task<ApplicationTypeDto?> GetByIdAsync(int id)
        {
            var type = _types.FirstOrDefault(t => t.ApplicationTypeID == id);
            return Task.FromResult(type);
        }

        public Task<int> AddAsync(ApplicationTypeDto type)
        {
            type.ApplicationTypeID = _types.Count > 0 ? _types.Max(t => t.ApplicationTypeID) + 1 : 1;
            _types.Add(type);
            return Task.FromResult(type.ApplicationTypeID);
        }

        public Task<bool> UpdateAsync(int id, ApplicationTypeDto type)
        {
            var existing = _types.FirstOrDefault(t => t.ApplicationTypeID == id);
            if (existing is null) return Task.FromResult(false);

            _types.Remove(existing);
            type.ApplicationTypeID = id;
            _types.Add(type);
            return Task.FromResult(true);
        }
    }
}