using DVLD.Api.Data.Configurations;
using DVLD.Api.Data.Entities;
using DVLD.Api.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Data
{
    /// <summary>
    /// Entity Framework Core <see cref="DbContext"/> for the DVLD database.
    /// </summary>
    /// <param name="options">The options used to configure the context (provider and connection).</param>
    internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : DbContext(options), IApplicationDbContext
    {
        /// <summary>
        /// Gets the set of people.
        /// </summary>
        public DbSet<Person> People => Set<Person>();

        /// <summary>
        /// Gets the set of user accounts.
        /// </summary>
        public DbSet<User> Users => Set<User>();

        /// <summary>
        /// Gets the set of application types.
        /// </summary>
        public DbSet<ApplicationType> ApplicationTypes => Set<ApplicationType>();

        /// <summary>
        /// Gets the set of test types.
        /// </summary>
        public DbSet<TestType> TestTypes => Set<TestType>();

        /// <summary>
        /// Gets the set of applications.
        /// </summary>
        public DbSet<Application> Applications => Set<Application>();

        /// <summary>
        /// Applies entity type configurations for the DVLD model.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TestTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
