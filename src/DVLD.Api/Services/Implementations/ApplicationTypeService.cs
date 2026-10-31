using DVLD.Api.Data.Interfaces;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.Services.Implementations
{
    /// <summary>
    /// Application service implementation for <see cref="ApplicationTypeDto"/> records.
    /// </summary>
    public class ApplicationTypeService : IApplicationTypeService
    {
        private readonly IApplicationTypeRepository _repository;
        private readonly ILogger<ApplicationTypeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationTypeService"/> class.
        /// </summary>
        /// <param name="repository">The application type repository.</param>
        /// <param name="logger">The logger.</param>
        public ApplicationTypeService(IApplicationTypeRepository repository, ILogger<ApplicationTypeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<ApplicationTypeDto>> GetAllTypesAsync()
        {
            _logger.LogInformation("Fetching all application types");
            return await _repository.GetAllAsync();
        }

        /// <inheritdoc />
        public async Task<ApplicationTypeDto?> GetTypeByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <inheritdoc />
        public async Task<ApplicationTypeDto> AddNewTypeAsync(ApplicationTypeDto type)
        {
            var newId = await _repository.AddAsync(type);
            type.ApplicationTypeID = newId;
            _logger.LogInformation("Added new application type: {Title}", type.ApplicationTypeTitle);
            return type;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateTypeAsync(int id, ApplicationTypeDto type)
        {
            return await _repository.UpdateAsync(id, type);
        }
    }
}
