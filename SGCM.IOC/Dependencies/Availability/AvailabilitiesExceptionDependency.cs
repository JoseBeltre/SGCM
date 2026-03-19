using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces.Availability;
using SGCM.Application.Services;
using SGCM.Domain.Repository;
using SGCM.Persistence.Repositories;

namespace SGCM.IOC.Dependencies.Availability
{
    public static class AvailabilityExceptionDependency
    {
        public static IServiceCollection AddAvailabilityException(this IServiceCollection services)
        {
            services.AddScoped<IAvailabilityExceptionService, AvailabilityExceptionService>();
            services.AddScoped<IAvailabilityExceptionRepository, AvailabilityExceptionRepository>();


            return services;
        }
    }
}
