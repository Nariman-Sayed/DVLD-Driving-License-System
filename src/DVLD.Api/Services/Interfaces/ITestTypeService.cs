using DVLD.Shared;

namespace DVLD.Api.Services.Interfaces
{
    /// <summary>
    /// Application service operations for <see cref="TestTypeDto"/> records.
    /// </summary>
    public interface ITestTypeService
    {
        /// <summary>
        /// Retrieves all test types.
        /// </summary>
        /// <returns>All test types.</returns>
        Task<List<TestTypeDto>> GetAllTestTypesAsync();

        /// <summary>
        /// Retrieves a single test type by identifier.
        /// </summary>
        /// <param name="id">The test type identifier.</param>
        /// <returns>The matching test type, or <see langword="null"/> when no test type exists.</returns>
        Task<TestTypeDto?> GetTestTypeByIdAsync(int id);

        /// <summary>
        /// Creates a new test type.
        /// </summary>
        /// <param name="type">The test type to create.</param>
        /// <returns>The created test type.</returns>
        Task<TestTypeDto> AddNewTestTypeAsync(TestTypeDto type);

        /// <summary>
        /// Updates an existing test type.
        /// </summary>
        /// <param name="id">The identifier of the test type to update.</param>
        /// <param name="type">The new values for the test type.</param>
        /// <returns><see langword="true"/> when the test type was updated; otherwise <see langword="false"/>.</returns>
        Task<bool> UpdateTestTypeAsync(int id, TestTypeDto type);
    }
}
