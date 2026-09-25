using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    public interface ITestTypeRepository
    {
        Task<List<TestTypeDto>> GetAllAsync();
        Task<TestTypeDto?> GetByIdAsync(int id);
        Task<int> AddAsync(TestTypeDto type);
        Task<bool> UpdateAsync(int id, TestTypeDto type);
    }
}