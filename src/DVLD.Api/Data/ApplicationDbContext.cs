using DVLD.Api.Data.Configurations;
using DVLD.Api.Data.Entities;
using DVLD.Api.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> People => Set<Person>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
