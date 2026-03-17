using Microsoft.Extensions.DependencyInjection;
using SGCM.Applicaction.Interfaces.Availability;
using SGCM.Applicaction.Services;

namespace SGCM.IOC.Dependencies.Availability
{
    public static class AvailabilityExceptionDependency
    {
        public static IServiceCollection Register(IServiceCollection services)
        {
            services.AddScoped<IAvailabilityExceptionService, AvailabilityExceptionService>();

            return services;
        }
    }
}
