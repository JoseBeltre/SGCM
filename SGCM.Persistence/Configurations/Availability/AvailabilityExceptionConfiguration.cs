using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCM.Domain.Entities;
using System.Numerics;

namespace SGCM.Persistence.Configurations
{
    public class AvailabilityExceptionConfiguration : IEntityTypeConfiguration<AvailabilityException> { }
    public void Configure(EntityTypeBuilder<AvailabilityException> builder)
        {
            builder.ToTable("AvailabilityExceptions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("ExceptionId");

            builder.Property(x => x.DoctorId)
                   .IsRequired();

            builder.Property(x => x.StartDate)
                   .IsRequired();

            builder.Property(x => x.EndDate)
                   .IsRequired();

            builder.Property(x => x.Reason)
                   .HasMaxLength(200);

            builder.Property(x => x.ExceptionType)
                   .HasMaxLength(30)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne<Doctor>()
                   .WithMany()
                   .HasForeignKey(x => x.DoctorId);
        }
    }
}
