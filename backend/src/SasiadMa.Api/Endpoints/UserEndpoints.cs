namespace SasiadMa.Api.Endpoints;

public static class UserEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users")
            .WithOpenApi();

        group.MapGet("/profile", () => Results.Ok(new { message = "User endpoints - coming soon" }))
            .WithName("GetProfile")
            .WithSummary("Get user profile")
            .RequireAuthorization();

        return app;
    }
}
