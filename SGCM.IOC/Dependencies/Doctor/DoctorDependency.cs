using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces;
using SGCM.Application.Services;
using SGCM.Domain.Repository;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;
using SGCM.Persistence.Repositories;

namespace SGCM.IOC.Dependencies.Doctor
{
    public static class DoctorDependency
    {
        public static IServiceCollection Register(IServiceCollection services)
        {
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IDoctorAppService, DoctorAppService>();
            return services;
        }
    }
}
