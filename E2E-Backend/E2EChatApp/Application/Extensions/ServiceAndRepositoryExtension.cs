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
        services.AddScoped<IFriendshipService, FriendshipService>();
        
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IAuthHelper, AuthHelper>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddSignalR();
        #endregion
        #region Repositories
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        
        #endregion
        
        return services;
    }
}
