namespace SasiadMa.Api.Endpoints;

public static class NotificationEndpoints
{
    public static WebApplication MapNotificationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/notifications")
            .WithTags("Notifications")
            .WithOpenApi();

        group.MapGet("/", () => Results.Ok(new { message = "Notification endpoints - coming soon" }))
            .WithName("GetNotifications")
            .WithSummary("Get notifications")
            .RequireAuthorization();

        group.MapGet("/settings", () => Results.Ok(new Dictionary<string, bool>
            {
                { "emailNotifications", true },
                { "pushNotifications", true },
                { "borrowRequests", true },
                { "lendingConfirmations", true },
                { "communityUpdates", true },
                { "reminders", true }
            }))
            .WithName("GetNotificationSettings")
            .WithSummary("Get notification settings")
            .RequireAuthorization();

        group.MapPut("/settings", (Dictionary<string, bool> settings) => Results.Ok())
            .WithName("UpdateNotificationSettings")
            .WithSummary("Update notification settings")
            .RequireAuthorization();

        return app;
    }
}
