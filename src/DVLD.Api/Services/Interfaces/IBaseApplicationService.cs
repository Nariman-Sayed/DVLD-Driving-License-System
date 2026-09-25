using DVLD.Shared;

namespace DVLD.Api.Services.Interfaces
{
    public interface IBaseApplicationService
    {
        Task<List<BaseApplicationDto>> GetAllApplicationsAsync();
        Task<BaseApplicationDto?> GetApplicationByIdAsync(int id);
        Task<BaseApplicationDto> AddNewApplicationAsync(CreateBaseApplicationRequest request, int createdByUserId);
        Task<bool> UpdateApplicationAsync(int id, BaseApplicationDto application);
    }
}