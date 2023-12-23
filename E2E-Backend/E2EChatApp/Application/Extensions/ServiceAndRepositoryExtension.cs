using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Application.Services;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Helpers;
using E2EChatApp.Infrastructure.Repositories;
namespace E2EChatApp.Application.Extensions;

public static class ServiceAndRepositoryExtension {
    public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services) {
        #region Services
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISecurityService, SecurityService>();
        
        #endregion
        #region Repositories
        
        services.AddScoped<IUserRepository, UserRepository>();
        
        #endregion

        services.AddScoped<IAuthHelper, AuthHelper>();
        
        return services;
    }
}
