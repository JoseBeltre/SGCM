using Microsoft.Extensions.DependencyInjection;
using SGCM.IOC.Dependencies.Notification;

namespace SGCM.IOC.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSGCM(IServiceCollection services)
        {
            services = NotificationDependency.Register(services);

            return services;
        }
    }
}
