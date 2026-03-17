using Microsoft.Extensions.DependencyInjection;
using SGCM.Applicaction.Interfaces.Availability;
using SGCM.Applicaction.Services.Availability;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.IOC.Dependencies.Availability
{
    public static class AvailabilityDependency
    {
        public static IServiceCollection Register(IServiceCollection services)
        {
            services.AddScoped<IAvailabilityService, AvailabilityService>();
            services.AddScoped<IAvailabilityDomainService, AvailabilityDomainService>();

            return services;
        }
    }
}
