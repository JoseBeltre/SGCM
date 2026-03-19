using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCM.Domain.Entities;

namespace SGCM.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("UserId");
            builder.Property(x => x.FullName)
                   .HasMaxLength(200)
                   .IsRequired();
            builder.Property(x => x.Email)
                   .HasMaxLength(150)
                   .IsRequired();
            builder.HasIndex(x => x.Email)
                   .IsUnique();
            builder.Property(x => x.Phone)
                   .HasMaxLength(20);
            builder.Property(x => x.PasswordHash)
                   .HasMaxLength(500)
                   .IsRequired();
            builder.Property(x => x.UserType)
                   .HasMaxLength(20)
                   .HasConversion<string>()
                   .IsRequired();
            builder.Property(x => x.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);
            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.LastAccess);
        }
    }
}