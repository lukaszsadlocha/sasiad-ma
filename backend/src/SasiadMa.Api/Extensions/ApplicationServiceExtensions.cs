using SasiadMa.Application.Interfaces;
using SasiadMa.Application.Services;

namespace SasiadMa.Api.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICommunityService, CommunityService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IItemService, ItemService>();

        // Other services will be added as they are implemented
        // services.AddScoped<IUserService, UserService>();
        // services.AddScoped<IBorrowService, BorrowService>();
        // services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}
