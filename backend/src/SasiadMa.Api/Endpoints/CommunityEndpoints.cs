using SasiadMa.Api.Extensions;
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

        group.MapGet("/{id:guid}", GetCommunityByIdAsync)
            .WithName("GetCommunityById")
            .WithSummary("Get community details by ID")
            .RequireAuthorization();

        group.MapPost("/join", JoinCommunityAsync)
            .WithName("JoinCommunity")
            .WithSummary("Join a community using invitation code")
            .RequireAuthorization();

        group.MapPost("/{id:guid}/leave", LeaveCommunityAsync)
            .WithName("LeaveCommunity")
            .WithSummary("Leave a community")
            .RequireAuthorization();

        group.MapPost("/{id:guid}/invitation-code", GenerateInvitationCodeAsync)
            .WithName("GenerateInvitationCode")
            .WithSummary("Generate new invitation code for community")
            .RequireAuthorization();

        group.MapGet("/{id:guid}/members", GetCommunityMembersAsync)
            .WithName("GetCommunityMembers")
            .WithSummary("Get community members")
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
            : Results.BadRequest(result.Errors.FirstOrDefault()?.Message);
    }

    private static async Task<IResult> GetCommunityByIdAsync(
        ICommunityService communityService,
        ClaimsPrincipal user,
        Guid id)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await communityService.GetByIdAsync(id, userId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.Errors.FirstOrDefault()?.Message);
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
            : Results.BadRequest(result.Errors.FirstOrDefault()?.Message);
    }

    private static async Task<IResult> LeaveCommunityAsync(
        ICommunityService communityService,
        ClaimsPrincipal user,
        Guid id)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await communityService.LeaveAsync(id, userId);

        return result.IsSuccess
            ? Results.Ok(new { message = "Successfully left community" })
            : Results.BadRequest(result.Errors.FirstOrDefault()?.Message);
    }

    private static async Task<IResult> GenerateInvitationCodeAsync(
        ICommunityService communityService,
        ClaimsPrincipal user,
        Guid id)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await communityService.GenerateInvitationCodeAsync(id, userId);

        return result.IsSuccess
            ? Results.Ok(new { invitationCode = result.Value })
            : Results.BadRequest(result.Errors.FirstOrDefault()?.Message);
    }

    private static async Task<IResult> GetCommunityMembersAsync(
        ICommunityService communityService,
        ClaimsPrincipal user,
        Guid id)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await communityService.GetMembersAsync(id, userId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors.FirstOrDefault()?.Message);
    }
}
