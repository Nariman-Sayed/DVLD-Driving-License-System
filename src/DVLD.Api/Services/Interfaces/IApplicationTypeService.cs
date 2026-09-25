using DVLD.Shared;

namespace DVLD.Api.Services.Interfaces
{
    public interface IApplicationTypeService
    {
        Task<List<ApplicationTypeDto>> GetAllTypesAsync();
        Task<ApplicationTypeDto?> GetTypeByIdAsync(int id);
        Task<ApplicationTypeDto> AddNewTypeAsync(ApplicationTypeDto type);
        Task<bool> UpdateTypeAsync(int id, ApplicationTypeDto type);
    }
}