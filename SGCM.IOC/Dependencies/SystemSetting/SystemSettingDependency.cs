using Microsoft.Extensions.DependencyInjection;
using SGCM.Applicaction.Interfaces;
using SGCM.Applicaction.Services;
using SGCM.Domain.Repository;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;
using SGCM.Persistence.Repositories;

namespace SGCM.IOC.Dependencies.SystemSetting
{
    public static class SystemSettingDependency
    {
        public static IServiceCollection Register(IServiceCollection services)
        {
            services.AddScoped<ISystemSettingService, SystemSettingService>();
            services.AddScoped<ISystemSettingRepository, SystemSettingRepository>();
            services.AddScoped<ISystemSettingAppService, SystemSettingAppService>();
            return services;
        }
    }
}
