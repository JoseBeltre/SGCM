using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCM.Domain.Entities;

namespace SGCM.Persistence.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("AppointmentId");
            builder.Property(x => x.PatientId)
                   .IsRequired();
            builder.Property(x => x.DoctorId)
                   .IsRequired();
            builder.Property(x => x.AppointmentDate)
                   .IsRequired();
            builder.Property(x => x.DurationMinutes)
                   .IsRequired()
                   .HasDefaultValue(30);
            builder.Property(x => x.Status)
                   .HasMaxLength(20)
                   .HasConversion<string>()
                   .IsRequired();
            builder.Property(x => x.ConsultationReason)
                   .HasMaxLength(500);
            builder.Property(x => x.DoctorNotes)
                   .HasMaxLength(1000);
            builder.Property(x => x.CancellationReason)
                   .HasMaxLength(300);
            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.ConfirmedAt);
            builder.Property(x => x.CompletedAt);
            builder.Property(x => x.CancelledAt);
            builder.HasOne<Patient>()
                   .WithMany()
                   .HasForeignKey(x => x.PatientId);
            builder.HasOne<Doctor>()
                   .WithMany()
                   .HasForeignKey(x => x.DoctorId);
        }
    }
}