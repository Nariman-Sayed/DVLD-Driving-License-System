using DVLD.Api.Data.Interfaces;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.Services.Implementations
{
    /// <summary>
    /// Application service implementation for <see cref="TestTypeDto"/> records.
    /// </summary>
    public class TestTypeService : ITestTypeService
    {
        private readonly ITestTypeRepository _repository;
        private readonly ILogger<TestTypeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestTypeService"/> class.
        /// </summary>
        /// <param name="repository">The test type repository.</param>
        /// <param name="logger">The logger.</param>
        public TestTypeService(ITestTypeRepository repository, ILogger<TestTypeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<TestTypeDto>> GetAllTestTypesAsync()
        {
            _logger.LogInformation("Fetching all test types");
            return await _repository.GetAllAsync();
        }

        /// <inheritdoc />
        public async Task<TestTypeDto?> GetTestTypeByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <inheritdoc />
        public async Task<TestTypeDto> AddNewTestTypeAsync(TestTypeDto type)
        {
            var newId = await _repository.AddAsync(type);
            type.TestTypeID = newId;
            _logger.LogInformation("Added new test type: {Title}", type.TestTypeTitle);
            return type;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateTestTypeAsync(int id, TestTypeDto type)
        {
            return await _repository.UpdateAsync(id, type);
        }
    }
}
