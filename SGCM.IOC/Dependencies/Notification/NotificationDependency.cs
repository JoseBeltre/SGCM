using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces.Notification;
using SGCM.Application.Services.Notification;
using SGCM.Domain.Repository;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;
using SGCM.Persistence.Repositories;

namespace SGCM.IOC.Dependencies.Notification
{
    public static class NotificationDependency
    {
        public static IServiceCollection AddNotification(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationDomainService, NotificationsDomainService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            return services;
        }
    }
}
