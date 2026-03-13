
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCM.Domain.Entities;

namespace SGCM.Persistence.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("AuditId");

            builder.Property(x => x.UserId)
                   .IsRequired();

            builder.Property(x => x.EntityType)
                   .HasMaxLength(50)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(x => x.EntityId)
                   .IsRequired();

            builder.Property(x => x.Action)
                   .HasMaxLength(50)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(x => x.PreviousValues)
                   .HasColumnType("nvarchar(max)");

            builder.Property(x => x.NewValues)
                   .HasColumnType("nvarchar(max)");

            builder.Property(x => x.IpAddress)
                   .HasMaxLength(50);

            builder.Property(x => x.UserAgent)
                   .HasMaxLength(500);

            builder.Property(x => x.ActionDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(x => x.UserId);
        }
    }
}
