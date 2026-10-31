using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    /// <summary>
    /// Persistence operations for <see cref="TestTypeDto"/> records.
    /// </summary>
    public interface ITestTypeRepository
    {
        /// <summary>
        /// Retrieves all test types.
        /// </summary>
        /// <returns>All test types; an empty list when none exist.</returns>
        Task<List<TestTypeDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a single test type by identifier.
        /// </summary>
        /// <param name="id">The test type identifier.</param>
        /// <returns>The matching test type, or <see langword="null"/> when no test type exists.</returns>
        Task<TestTypeDto?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new test type.
        /// </summary>
        /// <param name="type">The test type to create.</param>
        /// <returns>The generated test type identifier.</returns>
        Task<int> AddAsync(TestTypeDto type);

        /// <summary>
        /// Updates an existing test type.
        /// </summary>
        /// <param name="id">The identifier of the test type to update.</param>
        /// <param name="type">The new values for the test type.</param>
        /// <returns><see langword="true"/> when the test type was updated; otherwise <see langword="false"/>.</returns>
        Task<bool> UpdateAsync(int id, TestTypeDto type);
    }
}
