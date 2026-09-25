using DVLD.Api.Data.Interfaces;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.Services.Implementations
{
    public class TestTypeService : ITestTypeService
    {
        private readonly ITestTypeRepository _repository;
        private readonly ILogger<TestTypeService> _logger;

        public TestTypeService(ITestTypeRepository repository, ILogger<TestTypeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<TestTypeDto>> GetAllTestTypesAsync()
        {
            _logger.LogInformation("Fetching all test types");
            return await _repository.GetAllAsync();
        }

        public async Task<TestTypeDto?> GetTestTypeByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<TestTypeDto> AddNewTestTypeAsync(TestTypeDto type)
        {
            var newId = await _repository.AddAsync(type);
            type.TestTypeID = newId;
            _logger.LogInformation("Added new test type: {Title}", type.TestTypeTitle);
            return type;
        }

        public async Task<bool> UpdateTestTypeAsync(int id, TestTypeDto type)
        {
            return await _repository.UpdateAsync(id, type);
        }
    }
}