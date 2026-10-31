using DVLD.Shared;

namespace DVLD.Api.Services.Interfaces
{
    /// <summary>
    /// Application service operations for <see cref="ApplicationTypeDto"/> records.
    /// </summary>
    public interface IApplicationTypeService
    {
        /// <summary>
        /// Retrieves all application types.
        /// </summary>
        /// <returns>All application types.</returns>
        Task<List<ApplicationTypeDto>> GetAllTypesAsync();

        /// <summary>
        /// Retrieves a single application type by identifier.
        /// </summary>
        /// <param name="id">The application type identifier.</param>
        /// <returns>The matching application type, or <see langword="null"/> when no application type exists.</returns>
        Task<ApplicationTypeDto?> GetTypeByIdAsync(int id);

        /// <summary>
        /// Creates a new application type.
        /// </summary>
        /// <param name="type">The application type to create.</param>
        /// <returns>The created application type.</returns>
        Task<ApplicationTypeDto> AddNewTypeAsync(ApplicationTypeDto type);

        /// <summary>
        /// Updates an existing application type.
        /// </summary>
        /// <param name="id">The identifier of the application type to update.</param>
        /// <param name="type">The new values for the application type.</param>
        /// <returns><see langword="true"/> when the application type was updated; otherwise <see langword="false"/>.</returns>
        Task<bool> UpdateTypeAsync(int id, ApplicationTypeDto type);
    }
}
