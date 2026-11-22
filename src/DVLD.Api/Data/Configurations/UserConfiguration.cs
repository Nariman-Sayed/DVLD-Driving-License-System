using DVLD.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Api.Data.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.UserID);
            builder.Property(u => u.UserID).ValueGeneratedOnAdd();

            builder.Property(u => u.PersonID).IsRequired();
            builder.Property(u => u.Username).HasColumnName("UserName").HasMaxLength(20).IsRequired();
            builder.Property(u => u.Password).HasMaxLength(255).IsUnicode(false).IsRequired();
            builder.Property(u => u.IsActive).IsRequired();
        }
    }
}
