using DVLD.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Api.Data.Configurations
{
    internal sealed class ApplicationTypeConfiguration : IEntityTypeConfiguration<ApplicationType>
    {
        public void Configure(EntityTypeBuilder<ApplicationType> builder)
        {
            builder.ToTable("ApplicationTypes");

            builder.HasKey(t => t.ApplicationTypeID);
            builder.Property(t => t.ApplicationTypeID).ValueGeneratedOnAdd();

            builder.Property(t => t.ApplicationTypeTitle).HasMaxLength(150).IsRequired();
            builder.Property(t => t.ApplicationFees).HasColumnType("smallmoney").IsRequired();
        }
    }
}
