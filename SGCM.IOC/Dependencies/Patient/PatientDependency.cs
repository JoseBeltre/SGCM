using Microsoft.Extensions.DependencyInjection;
using SGCM.Applicaction.Interfaces;
using SGCM.Applicaction.Services;
using SGCM.Domain.Repository;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;
using SGCM.Persistence.Repositories;

namespace SGCM.IOC.Dependencies.Patient

{
    public static class PatientDependency
    {
        public static IServiceCollection Register(IServiceCollection services)
        {
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPatientAppService, PatientAppService>();
            return services;
        }
    }
}
