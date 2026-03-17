using Microsoft.Extensions.DependencyInjection;
using SGCM.IOC.Dependencies.Appointment;
using SGCM.IOC.Dependencies.AuditLog;
using SGCM.IOC.Dependencies.Availability;
using SGCM.IOC.Dependencies.Doctor;
using SGCM.IOC.Dependencies.Notification;
using SGCM.IOC.Dependencies.Specialty;

namespace SGCM.IOC.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSGCM(this IServiceCollection services)
        {
            return services
                .AddNotification()
                .AddAuditLog()
                .AddAvailability()
                .AddAvailabilityException()
                .AddSpecialty();
        }
    }
}
