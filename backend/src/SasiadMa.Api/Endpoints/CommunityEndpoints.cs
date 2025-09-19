using Microsoft.AspNetCore.Mvc;
using SasiadMa.Application.Interfaces;
using SasiadMa.Application.DTOs.Community;
using System.Security.Claims;

namespace SasiadMa.Api.Endpoints;

public static class CommunityEndpoints
{
    public static WebApplication MapCommunityEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/communities")
            .WithTags("Communities")
            .WithOpenApi();

        group.MapGet("/", GetUserCommunitiesAsync)
            .WithName("GetCommunities")
            .WithSummary("Get user communities")
            .RequireAuthorization();

        group.MapPost("/", CreateCommunityAsync)
            .WithName("CreateCommunity")
            .WithSummary("Create a new community")
            .RequireAuthorization();

        group.MapPost("/join", JoinCommunityAsync)
            .WithName("JoinCommunity")
            .WithSummary("Join a community using invitation code")
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
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> CreateCommunityAsync(
        ICommunityService communityService,
        ClaimsPrincipal user,
        [FromBody] CreateCommunityRequest request)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await communityService.CreateAsync(request, userId);

        return result.IsSuccess
            ? Results.Created($"/api/communities/{result.Value.Id}", result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> JoinCommunityAsync(
        ICommunityService communityService,
        ClaimsPrincipal user,
        [FromBody] JoinCommunityRequest request)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await communityService.JoinAsync(request, userId);

        return result.IsSuccess
            ? Results.Ok(new { message = "Successfully joined community" })
            : Results.BadRequest(result.Error);
    }
}
