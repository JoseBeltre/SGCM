using Microsoft.Extensions.DependencyInjection;
using SGCM.Domain.Repository;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;
using SGCM.Persistence.Repositories;

namespace SGCM.IOC.Dependencies.AuditLog
{
    public static class AuditLogDependency
    {
        public static IServiceCollection AddAuditLog(this IServiceCollection services)
        {
            services.AddScoped<IAuditLogDomainService, AuditLogDomainService>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();

            return services;
        }
    }
}
