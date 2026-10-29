using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    /// <summary>
    /// Persistence operations for <see cref="BaseApplicationDto"/> records.
    /// </summary>
    public interface IBaseApplicationRepository
    {
        /// <summary>
        /// Retrieves all base applications.
        /// </summary>
        /// <returns>All base applications; an empty list when none exist.</returns>
        Task<List<BaseApplicationDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a single base application by identifier.
        /// </summary>
        /// <param name="id">The base application identifier.</param>
        /// <returns>The matching base application, or <see langword="null"/> when no base application exists.</returns>
        Task<BaseApplicationDto?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new base application.
        /// </summary>
        /// <param name="application">The base application to create.</param>
        /// <returns>The generated base application identifier.</returns>
        Task<int> AddAsync(BaseApplicationDto application);

        /// <summary>
        /// Updates an existing base application.
        /// </summary>
        /// <param name="id">The identifier of the base application to update.</param>
        /// <param name="application">The new values for the base application.</param>
        /// <returns><see langword="true"/> when the base application was updated; otherwise <see langword="false"/>.</returns>
        Task<bool> UpdateAsync(int id, BaseApplicationDto application);

        /// <summary>
        /// Retrieves the identifier of an active application for the given applicant and application type.
        /// </summary>
        /// <param name="applicantPersonId">The applicant person identifier.</param>
        /// <param name="applicationTypeId">The application type identifier.</param>
        /// <returns>The active application identifier, or <see langword="null"/> when none exists.</returns>
        Task<int?> GetActiveApplicationIdAsync(int applicantPersonId, int applicationTypeId);
    }
}
