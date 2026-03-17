using Microsoft.Extensions.DependencyInjection;
using SGCM.Applicaction.Interfaces.Specialty;
using SGCM.Applicaction.Services.Specialty;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.IOC.Dependencies.Specialty
{
    public static class SpecialtyDependency
    {
        public static IServiceCollection AddSpecialty(this IServiceCollection services)
        {
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<ISpecialtiyDomainService, SpecialtyDomainService>();

            return services;
        }
    }
}
