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
        /// Applies entity type configurations for the DVLD model.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
