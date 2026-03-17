using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCM.Domain.Entities;

namespace SGCM.Persistence.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("DoctorId");
            builder.Property(x => x.UserId)
                   .IsRequired();
            builder.Property(x => x.SpecialtyId)
                   .IsRequired();
            builder.Property(x => x.NationalId)
                   .HasMaxLength(20)
                   .IsRequired();
            builder.HasIndex(x => x.NationalId)
                   .IsUnique();
            builder.Property(x => x.LicenseNumber)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.HasIndex(x => x.LicenseNumber)
                   .IsUnique();
            builder.Property(x => x.HireDate)
                   .IsRequired();
            builder.Property(x => x.AssignedOffice)
                   .HasMaxLength(50);
            builder.Property(x => x.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);
            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(x => x.UserId);
            builder.HasOne<Specialty>()
                   .WithMany()
                   .HasForeignKey(x => x.SpecialtyId);
        }
    }
}