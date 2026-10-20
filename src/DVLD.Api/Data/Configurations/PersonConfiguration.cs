using DVLD.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Api.Data.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People");

            builder.HasKey(p => p.PersonID);
            builder.Property(p => p.PersonID).ValueGeneratedOnAdd();

            builder.Property(p => p.NationalNo).HasMaxLength(20).IsRequired();
            builder.Property(p => p.FirstName).HasMaxLength(20).IsRequired();
            builder.Property(p => p.SecondName).HasMaxLength(20).IsRequired();
            builder.Property(p => p.ThirdName).HasMaxLength(20);
            builder.Property(p => p.LastName).HasMaxLength(20).IsRequired();
            builder.Property(p => p.DateOfBirth).HasColumnType("datetime").IsRequired();
            builder.Property(p => p.Gendor).HasColumnType("tinyint").IsRequired();
            builder.Property(p => p.Address).HasMaxLength(500).IsRequired();
            builder.Property(p => p.Phone).HasMaxLength(20).IsRequired();
            builder.Property(p => p.Email).HasMaxLength(50);
            builder.Property(p => p.NationalityCountryID).IsRequired();
            builder.Property(p => p.ImagePath).HasMaxLength(250);
        }
    }
}
