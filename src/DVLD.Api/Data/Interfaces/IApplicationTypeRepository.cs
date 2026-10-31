using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    /// <summary>
    /// Persistence operations for <see cref="ApplicationTypeDto"/> records.
    /// </summary>
    public interface IApplicationTypeRepository
    {
        /// <summary>
        /// Retrieves all application types.
        /// </summary>
        /// <returns>All application types; an empty list when none exist.</returns>
        Task<List<ApplicationTypeDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a single application type by identifier.
        /// </summary>
        /// <param name="id">The application type identifier.</param>
        /// <returns>The matching application type, or <see langword="null"/> when no application type exists.</returns>
        Task<ApplicationTypeDto?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new application type.
        /// </summary>
        /// <param name="type">The application type to create.</param>
        /// <returns>The generated application type identifier.</returns>
        Task<int> AddAsync(ApplicationTypeDto type);

        /// <summary>
        /// Updates an existing application type.
        /// </summary>
        /// <param name="id">The identifier of the application type to update.</param>
        /// <param name="type">The new values for the application type.</param>
        /// <returns><see langword="true"/> when the application type was updated; otherwise <see langword="false"/>.</returns>
        Task<bool> UpdateAsync(int id, ApplicationTypeDto type);
    }
}
