using Microsoft.Extensions.DependencyInjection;
using SGCM.IOC.Dependencies.AuditLog;
using SGCM.IOC.Dependencies.Availability;
using SGCM.IOC.Dependencies.Notification;

namespace SGCM.IOC.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSGCM(IServiceCollection services)
        {
            services = NotificationDependency.Register(services);
            services = AvailabilityExceptionDependency.Register(services);
            services = AvailabilityDependency.Register(services);
            services = AuditLogDependency.Register(services);

            return services;
        }
    }
}
