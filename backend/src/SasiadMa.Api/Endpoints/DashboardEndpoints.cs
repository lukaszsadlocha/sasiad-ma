using SasiadMa.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using SasiadMa.Application.Interfaces;
using System.Security.Claims;

namespace SasiadMa.Api.Endpoints;

public static class DashboardEndpoints
{
    public static WebApplication MapDashboardEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/dashboard")
            .WithTags("Dashboard")
            .WithOpenApi();

        group.MapGet("/stats", GetDashboardStatsAsync)
            .WithName("GetDashboardStats")
            .WithSummary("Get user dashboard statistics")
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> GetDashboardStatsAsync(
        IDashboardService dashboardService,
        ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await dashboardService.GetUserDashboardStatsAsync(userId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors.FirstOrDefault()?.Message);
    }
}