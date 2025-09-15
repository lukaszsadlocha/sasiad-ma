namespace SasiadMa.Api.Endpoints;

public static class ItemEndpoints
{
    public static WebApplication MapItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/items")
            .WithTags("Items")
            .WithOpenApi();

        group.MapGet("/", () => Results.Ok(new { message = "Item endpoints - coming soon" }))
            .WithName("GetItems")
            .WithSummary("Get items")
            .RequireAuthorization();

        return app;
    }
}
