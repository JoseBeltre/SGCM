using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces.Availability;
using SGCM.Application.Services.Availability;
using SGCM.Domain.Repository;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;
using SGCM.Persistence.Repositories;

namespace SGCM.IOC.Dependencies.Availability
{
    public static class AvailabilityDependency
    {
        public static IServiceCollection AddAvailability(this IServiceCollection services)
        {
            services.AddScoped<IAvailabilityService, AvailabilityService>();
            services.AddScoped<IAvailabilityDomainService, AvailabilityDomainService>();
            services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();


            return services;
        }
    }
}
