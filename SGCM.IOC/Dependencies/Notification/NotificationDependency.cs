using Microsoft.Extensions.DependencyInjection;
using SGCM.Applicaction.Interfaces.Notification;
using SGCM.Applicaction.Services.Notification;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.IOC.Dependencies.Notification
{
    public static class NotificationDependency
    {
        public static IServiceCollection Register(IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationDomainService, NotificationsDomainService>();

            return services;
        }
    }
}
