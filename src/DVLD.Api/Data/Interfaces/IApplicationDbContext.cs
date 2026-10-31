using DVLD.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Data.Interfaces
{
    /// <summary>
    /// Abstraction over the DVLD database used by repositories, exposing the entity sets
    /// and unit-of-work operation they require.
    /// </summary>
    internal interface IApplicationDbContext
    {
        /// <summary>
        /// Gets the set of people.
        /// </summary>
        DbSet<Person> People { get; }

        /// <summary>
        /// Gets the set of user accounts.
        /// </summary>
        DbSet<User> Users { get; }

        /// <summary>
        /// Gets the set of application types.
        /// </summary>
        DbSet<ApplicationType> ApplicationTypes { get; }

        /// <summary>
        /// Gets the set of test types.
        /// </summary>
        DbSet<TestType> TestTypes { get; }

        /// <summary>
        /// Gets the set of applications.
        /// </summary>
        DbSet<Application> Applications { get; }

        /// <summary>
        /// Persists all pending changes to the database.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
