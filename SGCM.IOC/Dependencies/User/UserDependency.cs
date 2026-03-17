using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces;
using SGCM.Application.Services;
using SGCM.Domain.Repository;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces;
using SGCM.Persistence.Repositories;

namespace SGCM.IOC.Dependencies.User
{
    public static class UserDependency
    {
        public static IServiceCollection Register(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAppService, UserAppService>();
            return services;
        }
    }
}
