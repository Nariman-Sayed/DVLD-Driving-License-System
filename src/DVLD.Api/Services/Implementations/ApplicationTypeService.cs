using DVLD.Api.Data.Interfaces;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.Services.Implementations
{
    public class ApplicationTypeService : IApplicationTypeService
    {
        private readonly IApplicationTypeRepository _repository;
        private readonly ILogger<ApplicationTypeService> _logger;

        public ApplicationTypeService(IApplicationTypeRepository repository, ILogger<ApplicationTypeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<ApplicationTypeDto>> GetAllTypesAsync()
        {
            _logger.LogInformation("Fetching all application types");
            return await _repository.GetAllAsync();
        }

        public async Task<ApplicationTypeDto?> GetTypeByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ApplicationTypeDto> AddNewTypeAsync(ApplicationTypeDto type)
        {
            var newId = await _repository.AddAsync(type);
            type.ApplicationTypeID = newId;
            _logger.LogInformation("Added new application type: {Title}", type.ApplicationTypeTitle);
            return type;
        }

        public async Task<bool> UpdateTypeAsync(int id, ApplicationTypeDto type)
        {
            return await _repository.UpdateAsync(id, type);
        }
    }
}