using DVLD.Api.Data.Interfaces;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.Services.Implementations
{
    public class BaseApplicationService : IBaseApplicationService
    {
        private readonly IBaseApplicationRepository _repository;
        private readonly ILogger<BaseApplicationService> _logger;

        public BaseApplicationService(IBaseApplicationRepository repository, ILogger<BaseApplicationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<BaseApplicationDto>> GetAllApplicationsAsync()
        {
            _logger.LogInformation("Fetching all applications");
            return await _repository.GetAllAsync();
        }

        public async Task<BaseApplicationDto?> GetApplicationByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<BaseApplicationDto> AddNewApplicationAsync(BaseApplicationDto application)
        {
            var newId = await _repository.AddAsync(application);
            application.ApplicationID = newId;
            _logger.LogInformation("Added new application: {ApplicationId}", application.ApplicationID);
            return application;
        }

        public async Task<bool> UpdateApplicationAsync(int id, BaseApplicationDto application)
        {
            return await _repository.UpdateAsync(id, application);
        }
    }
}