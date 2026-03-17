using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGCM.Domain.Entities;

namespace SGCM.Persistence.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("PatientId");
            builder.Property(x => x.UserId)
                   .IsRequired();
            builder.Property(x => x.NationalId)
                   .HasMaxLength(20)
                   .IsRequired();
            builder.HasIndex(x => x.NationalId)
                   .IsUnique();
            builder.Property(x => x.DateOfBirth)
                   .IsRequired();
            builder.Property(x => x.Address)
                   .HasMaxLength(300);
            builder.Property(x => x.Gender)
                   .HasMaxLength(10)
                   .HasConversion<string>();
            builder.Property(x => x.EmergencyPhone)
                   .HasMaxLength(20);
            builder.Property(x => x.EmergencyContact)
                   .HasMaxLength(200);
            builder.Property(x => x.InsuranceNumber)
                   .HasMaxLength(50);
            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(x => x.UserId);
        }
    }
}