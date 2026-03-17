using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces.Availability;
using SGCM.Application.Services;

namespace SGCM.IOC.Dependencies.Availability
{
    public static class AvailabilityExceptionDependency
    {
        public static IServiceCollection AddAvailabilityException(this IServiceCollection services)
        {
            services.AddScoped<IAvailabilityExceptionService, AvailabilityExceptionService>();

            return services;
        }
    }
}
