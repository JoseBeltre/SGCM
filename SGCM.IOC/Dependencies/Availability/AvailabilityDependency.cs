using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces.Availability;
using SGCM.Application.Services.Availability;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.IOC.Dependencies.Availability
{
    public static class AvailabilityDependency
    {
        public static IServiceCollection AddAvailability(this IServiceCollection services)
        {
            services.AddScoped<IAvailabilityService, AvailabilityService>();
            services.AddScoped<IAvailabilityDomainService, AvailabilityDomainService>();

            return services;
        }
    }
}
