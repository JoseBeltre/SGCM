using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces.Authentication;
using SGCM.Application.Services.Authentication;

namespace SGCM.IOC.Dependencies.Authentication
{
    public static class AuthenticationDependency
    {
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
