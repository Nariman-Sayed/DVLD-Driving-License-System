using DVLD.Api.Data.Interfaces;
using DVLD.Api.Exceptions;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.Services.Implementations
{
    /// <summary>
    /// Application service implementation for <see cref="BaseApplicationDto"/> records.
    /// </summary>
    public class BaseApplicationService : IBaseApplicationService
    {
        private readonly IBaseApplicationRepository _repository;
        private readonly IApplicationTypeRepository _applicationTypeRepository;
        private readonly ILogger<BaseApplicationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApplicationService"/> class.
        /// </summary>
        /// <param name="repository">The base application repository.</param>
        /// <param name="applicationTypeRepository">The application type repository.</param>
        /// <param name="logger">The logger.</param>
        public BaseApplicationService(
            IBaseApplicationRepository repository,
            IApplicationTypeRepository applicationTypeRepository,
            ILogger<BaseApplicationService> logger)
        {
            _repository = repository;
            _applicationTypeRepository = applicationTypeRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<BaseApplicationDto>> GetAllApplicationsAsync()
        {
            _logger.LogInformation("Fetching all applications");
            return await _repository.GetAllAsync();
        }

        /// <inheritdoc />
        public async Task<BaseApplicationDto?> GetApplicationByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <inheritdoc />
        public async Task<BaseApplicationDto> AddNewApplicationAsync(CreateBaseApplicationRequest request, int createdByUserId)
        {
            var activeApplicationId = await _repository.GetActiveApplicationIdAsync(
                request.ApplicantPersonID, request.ApplicationTypeID);

            if (activeApplicationId is not null)
            {
                throw new ApplicationValidationException(
                    $"Person with ID {request.ApplicantPersonID} already has an active application for ApplicationType ID {request.ApplicationTypeID}.");
            }

            var applicationType = await _applicationTypeRepository.GetByIdAsync(request.ApplicationTypeID);
            if (applicationType is null)
            {
                throw new ApplicationValidationException(
                    $"ApplicationType with ID {request.ApplicationTypeID} not found.");
            }

            var application = new BaseApplicationDto
            {
                ApplicantPersonID = request.ApplicantPersonID,
                ApplicationTypeID = request.ApplicationTypeID,
                ApplicationDate = DateTime.UtcNow,
                ApplicationStatus = 1, // New
                LastStatusDate = DateTime.UtcNow,
                PaidFees = applicationType.ApplicationFees,
                CreatedByUserID = createdByUserId
            };

            var newId = await _repository.AddAsync(application);
            application.ApplicationID = newId;
            _logger.LogInformation("Added new application: {ApplicationId}", application.ApplicationID);
            return application;
        }

        /// <inheritdoc />
        // Deferred: enforce a New -> Cancelled/Completed state machine instead of
        // accepting a free-form status here (flagged for a later phase).
        public async Task<bool> UpdateApplicationAsync(int id, BaseApplicationDto application)
        {
            return await _repository.UpdateAsync(id, application);
        }
    }
}
