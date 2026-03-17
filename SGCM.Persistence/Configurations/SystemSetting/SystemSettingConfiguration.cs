using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCM.Domain.Entities;

namespace SGCM.Persistence.Configurations
{
    public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
    {
        public void Configure(EntityTypeBuilder<SystemSetting> builder)
        {
            builder.ToTable("SystemSettings");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("SettingId");
            builder.Property(x => x.SettingKey)
                   .HasMaxLength(100)
                   .IsRequired();
            builder.HasIndex(x => x.SettingKey)
                   .IsUnique();
            builder.Property(x => x.SettingValue)
                   .HasMaxLength(500)
                   .IsRequired();
            builder.Property(x => x.Description)
                   .HasMaxLength(300);
            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}