using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces;
using SGCM.Application.Services;
using SGCM.Domain.Repository;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;
using SGCM.Persistence.Repositories;

namespace SGCM.IOC.Dependencies.Patient

{
    public static class PatientDependency
    {
        public static IServiceCollection AddPatient(this IServiceCollection services)
        {
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPatientAppService, PatientAppService>();
            return services;
        }
    }
}
