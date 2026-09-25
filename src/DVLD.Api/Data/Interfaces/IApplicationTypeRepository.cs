using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    public interface IApplicationTypeRepository
    {
        Task<List<ApplicationTypeDto>> GetAllAsync();
        Task<ApplicationTypeDto?> GetByIdAsync(int id);
        Task<int> AddAsync(ApplicationTypeDto type);
        Task<bool> UpdateAsync(int id, ApplicationTypeDto type);
    }
}