using SasiadMa.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using SasiadMa.Application.Interfaces;
using System.Security.Claims;

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

        group.MapGet("/communities", GetUserCommunitiesAsync)
            .WithName("GetUserCommunities")
            .WithSummary("Get communities user is member of")
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> GetUserCommunitiesAsync(
        ICommunityService communityService,
        ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await communityService.GetUserCommunitiesAsync(userId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors.FirstOrDefault()?.Message);
    }
}
