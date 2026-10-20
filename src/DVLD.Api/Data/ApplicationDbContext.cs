using DVLD.Api.Data.Configurations;
using DVLD.Api.Data.Entities;
using DVLD.Api.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Data
{
    internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : DbContext(options), IApplicationDbContext
    {
        public DbSet<Person> People => Set<Person>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
