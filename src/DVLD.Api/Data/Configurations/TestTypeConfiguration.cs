using DVLD.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Api.Data.Configurations
{
    internal sealed class TestTypeConfiguration : IEntityTypeConfiguration<TestType>
    {
        public void Configure(EntityTypeBuilder<TestType> builder)
        {
            builder.ToTable("TestTypes");

            builder.HasKey(t => t.TestTypeID);
            builder.Property(t => t.TestTypeID).ValueGeneratedOnAdd();

            builder.Property(t => t.TestTypeTitle).HasMaxLength(100).IsRequired();
            builder.Property(t => t.TestTypeDescription).HasMaxLength(500).IsRequired();
            builder.Property(t => t.TestTypeFees).HasColumnType("smallmoney").IsRequired();
        }
    }
}
