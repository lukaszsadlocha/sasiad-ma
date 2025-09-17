using SasiadMa.Core.Interfaces;
using SasiadMa.Infrastructure.Data;
using SasiadMa.Infrastructure.Repositories;
using SasiadMa.Infrastructure.Services;
using SasiadMa.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SasiadMa.Api.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext with InMemory database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("SasiadMaInMemoryDb"));

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICommunityRepository, CommunityRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IBorrowRequestRepository, BorrowRequestRepository>();
        services.AddScoped<IItemRequestRepository, ItemRequestRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

        // Register infrastructure services
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
