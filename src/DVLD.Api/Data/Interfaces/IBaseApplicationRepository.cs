using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    public interface IBaseApplicationRepository
    {
        Task<List<BaseApplicationDto>> GetAllAsync();
        Task<BaseApplicationDto?> GetByIdAsync(int id);
        Task<int> AddAsync(BaseApplicationDto application);
        Task<bool> UpdateAsync(int id, BaseApplicationDto application);
        Task<int?> GetActiveApplicationIdAsync(int applicantPersonId, int applicationTypeId);
    }
}