using DVLD.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Api.Data.Configurations
{
    internal sealed class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.ToTable("Applications");

            builder.HasKey(a => a.ApplicationID);
            builder.Property(a => a.ApplicationID).ValueGeneratedOnAdd();

            builder.Property(a => a.ApplicantPersonID).IsRequired();
            builder.Property(a => a.ApplicationDate).HasColumnType("datetime").IsRequired();
            builder.Property(a => a.ApplicationTypeID).IsRequired();
            builder.Property(a => a.ApplicationStatus).HasColumnType("tinyint").IsRequired();
            builder.Property(a => a.LastStatusDate).HasColumnType("datetime").IsRequired();
            builder.Property(a => a.PaidFees).HasColumnType("smallmoney").IsRequired();
            builder.Property(a => a.CreatedByUserID).IsRequired();
        }
    }
}
