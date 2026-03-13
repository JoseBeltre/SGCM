
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCM.Domain.Entities;

namespace SGCM.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("NotificationId");

            builder.Property(x => x.AppointmentId)
                   .IsRequired();

            builder.Property(x => x.UserId)
                   .IsRequired();

            builder.Property(x => x.NotificationType)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.EventType)
                   .HasMaxLength(30)
                   .IsRequired();

            builder.Property(x => x.Subject)
                   .HasMaxLength(200);

            builder.Property(x => x.Message)
                   .HasMaxLength(1000)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .HasMaxLength(20)
                   .HasDefaultValue("Pendiente")
                   .IsRequired();

            builder.Property(x => x.SentAt);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.SendAttempts)
                   .HasDefaultValue(0)
                   .IsRequired();

            builder.Property(x => x.ErrorDetail)
                   .HasMaxLength(500);

            builder.HasOne<Appointment>()
                   .WithMany()
                   .HasForeignKey(x => x.AppointmentId);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(x => x.UserId);
        }
    }
    }
}
