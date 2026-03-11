using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCM.Domain.Entities;
using System.Numerics;

namespace SGCM.Persistence.Configurations
{
    public class AvailabilityConfiguration : IEntityTypeConfiguration<Availability>
    {
        public void Configure(EntityTypeBuilder<Availability> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("AvailabilityId");

            builder.Property(x => x.DoctorId)
                   .IsRequired();

            builder.Property(x => x.DayOfWeek)
                   .HasMaxLength(15)
                   .IsRequired();

            builder.Property(x => x.StartTime)
                   .IsRequired();

            builder.Property(x => x.EndTime)
                   .IsRequired();

            builder.Property(x => x.AppointmentDuration)
                   .HasDefaultValue(30)
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.UpdatedAt);

            //builder.HasOne<Doctor>()
            //       .WithMany()
            //       .HasForeignKey(x => x.DoctorId);
        }

    }
}
