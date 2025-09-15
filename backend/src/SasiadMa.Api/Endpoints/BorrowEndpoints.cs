namespace SasiadMa.Api.Endpoints;

public static class BorrowEndpoints
{
    public static WebApplication MapBorrowEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/borrows")
            .WithTags("Borrows")
            .WithOpenApi();

        group.MapGet("/", () => Results.Ok(new { message = "Borrow endpoints - coming soon" }))
            .WithName("GetBorrowRequests")
            .WithSummary("Get borrow requests")
            .RequireAuthorization();

        return app;
    }
}
