using SasiadMa.Api.Endpoints;

namespace SasiadMa.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApiMiddleware(this WebApplication app)
    {
        // Configure the HTTP request pipeline
        // Enable Swagger in all environments for now (development setup)
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Sasiad-Ma API is running!");

        // Map endpoint groups
        app.MapAuthEndpoints();
        app.MapUserEndpoints();
        app.MapCommunityEndpoints();
        app.MapItemEndpoints();
        app.MapBorrowEndpoints();
        app.MapNotificationEndpoints();

        return app;
    }
}
