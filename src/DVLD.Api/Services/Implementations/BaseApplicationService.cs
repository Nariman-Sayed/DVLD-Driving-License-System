using DVLD.Api.Data.Interfaces;
using DVLD.Api.Exceptions;
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

        public async Task<BaseApplicationDto> AddNewApplicationAsync(CreateBaseApplicationRequest request, int createdByUserId)
        {
            var activeApplicationId = await _repository.GetActiveApplicationIdAsync(
                request.ApplicantPersonID, request.ApplicationTypeID);

            if (activeApplicationId is not null)
            {
                throw new ApplicationValidationException(
                    $"Person with ID {request.ApplicantPersonID} already has an active application for ApplicationType ID {request.ApplicationTypeID}.");
            }

            var application = new BaseApplicationDto
            {
                ApplicantPersonID = request.ApplicantPersonID,
                ApplicationTypeID = request.ApplicationTypeID,
                ApplicationDate = DateTime.UtcNow,
                ApplicationStatus = 1, // New
                LastStatusDate = DateTime.UtcNow,
                CreatedByUserID = createdByUserId
            };

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