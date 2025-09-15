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

        return app;
    }
}
