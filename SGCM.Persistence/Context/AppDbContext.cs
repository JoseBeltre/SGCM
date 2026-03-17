using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities;

namespace SGCM.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<AvailabilityException> AvailabilityExceptions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Specialty> Specialty { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
