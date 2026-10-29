using DVLD.Shared;

namespace DVLD.Api.Services.Interfaces
{
    /// <summary>
    /// Application service operations for <see cref="BaseApplicationDto"/> records.
    /// </summary>
    public interface IBaseApplicationService
    {
        /// <summary>
        /// Retrieves all base applications.
        /// </summary>
        /// <returns>All base applications.</returns>
        Task<List<BaseApplicationDto>> GetAllApplicationsAsync();

        /// <summary>
        /// Retrieves a single base application by identifier.
        /// </summary>
        /// <param name="id">The base application identifier.</param>
        /// <returns>The matching base application, or <see langword="null"/> when no base application exists.</returns>
        Task<BaseApplicationDto?> GetApplicationByIdAsync(int id);

        /// <summary>
        /// Creates a new base application.
        /// </summary>
        /// <param name="request">The base application to create.</param>
        /// <param name="createdByUserId">The identifier of the user creating the application.</param>
        /// <returns>The created base application.</returns>
        Task<BaseApplicationDto> AddNewApplicationAsync(CreateBaseApplicationRequest request, int createdByUserId);

        /// <summary>
        /// Updates an existing base application.
        /// </summary>
        /// <param name="id">The identifier of the base application to update.</param>
        /// <param name="application">The new values for the base application.</param>
        /// <returns><see langword="true"/> when the base application was updated; otherwise <see langword="false"/>.</returns>
        Task<bool> UpdateApplicationAsync(int id, BaseApplicationDto application);
    }
}
