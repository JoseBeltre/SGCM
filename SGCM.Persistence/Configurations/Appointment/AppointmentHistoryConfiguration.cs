using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCM.Domain.Entities;

namespace SGCM.Persistence.Configurations
{
    public class AppointmentHistoryConfiguration : IEntityTypeConfiguration<AppointmentHistory>
    {
        public void Configure(EntityTypeBuilder<AppointmentHistory> builder)
        {
            builder.ToTable("AppointmentHistory");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("HistoryId");
            builder.Property(x => x.AppointmentId)
                   .IsRequired();
            builder.Property(x => x.PreviousStatus)
                   .HasMaxLength(20);
            builder.Property(x => x.NewStatus)
                   .HasMaxLength(20)
                   .IsRequired();
            builder.Property(x => x.ModifiedByUserId)
                   .IsRequired();
            builder.Property(x => x.Notes)
                   .HasMaxLength(500);
            builder.Property(x => x.RecordedAt)
                   .HasDefaultValueSql("GETDATE()");
            builder.HasOne<Appointment>()
                   .WithMany()
                   .HasForeignKey(x => x.AppointmentId);
            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(x => x.ModifiedByUserId);
        }
    }
}