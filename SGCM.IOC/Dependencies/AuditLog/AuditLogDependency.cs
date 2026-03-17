using Microsoft.Extensions.DependencyInjection;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.IOC.Dependencies.AuditLog
{
    public static class AuditLogDependency
    {
        public static IServiceCollection AddAuditLog(this IServiceCollection services)
        {
            services.AddScoped<IAuditLogDomainService, AuditLogDomainService>();

            return services;
        }
    }
}
