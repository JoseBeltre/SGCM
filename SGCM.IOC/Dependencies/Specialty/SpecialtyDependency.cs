using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces.Specialty;
using SGCM.Application.Services.Specialty;
using SGCM.Domain.Repository;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;
using SGCM.Persistence.Repositories;

namespace SGCM.IOC.Dependencies.Specialty
{
    public static class SpecialtyDependency
    {
        public static IServiceCollection AddSpecialty(this IServiceCollection services)
        {
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<ISpecialtyDomainService, SpecialtyDomainService>();
            services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();

            return services;
        }
    }
}
