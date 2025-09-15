namespace SasiadMa.Api.Endpoints;

public static class CommunityEndpoints
{
    public static WebApplication MapCommunityEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/communities")
            .WithTags("Communities")
            .WithOpenApi();

        group.MapGet("/", () => Results.Ok(new { message = "Community endpoints - coming soon" }))
            .WithName("GetCommunities")
            .WithSummary("Get user communities")
            .RequireAuthorization();

        return app;
    }
}
