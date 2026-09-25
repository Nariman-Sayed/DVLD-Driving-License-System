using DVLD.Shared;

namespace DVLD.Api.Services.Interfaces
{
    public interface ITestTypeService
    {
        Task<List<TestTypeDto>> GetAllTestTypesAsync();
        Task<TestTypeDto?> GetTestTypeByIdAsync(int id);
        Task<TestTypeDto> AddNewTestTypeAsync(TestTypeDto type);
        Task<bool> UpdateTestTypeAsync(int id, TestTypeDto type);
    }
}