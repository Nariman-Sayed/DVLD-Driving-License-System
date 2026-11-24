using DVLD.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Api.Data.Configurations
{
    internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");

            builder.HasKey(c => c.CountryID);
            builder.Property(c => c.CountryID).ValueGeneratedOnAdd();

            builder.Property(c => c.CountryName).HasMaxLength(50).IsRequired();
        }
    }
}
