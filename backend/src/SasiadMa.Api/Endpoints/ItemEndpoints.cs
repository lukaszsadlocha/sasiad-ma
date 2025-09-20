using Microsoft.AspNetCore.Mvc;
using SasiadMa.Application.Interfaces;
using SasiadMa.Application.DTOs.Item;
using System.Security.Claims;

namespace SasiadMa.Api.Endpoints;

public static class ItemEndpoints
{
    public static WebApplication MapItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/items")
            .WithTags("Items")
            .WithOpenApi();

        group.MapGet("/", GetItemsAsync)
            .WithName("GetItems")
            .WithSummary("Get items")
            .RequireAuthorization();

        group.MapGet("/{id:guid}", GetItemByIdAsync)
            .WithName("GetItemById")
            .WithSummary("Get item by ID")
            .RequireAuthorization();

        group.MapPost("/", CreateItemAsync)
            .WithName("CreateItem")
            .WithSummary("Create a new item")
            .RequireAuthorization();

        group.MapPut("/{id:guid}", UpdateItemAsync)
            .WithName("UpdateItem")
            .WithSummary("Update an item")
            .RequireAuthorization();

        group.MapDelete("/{id:guid}", DeleteItemAsync)
            .WithName("DeleteItem")
            .WithSummary("Delete an item")
            .RequireAuthorization();

        group.MapGet("/search", SearchItemsAsync)
            .WithName("SearchItems")
            .WithSummary("Search items")
            .RequireAuthorization();

        group.MapGet("/categories", GetCategoriesAsync)
            .WithName("GetCategories")
            .WithSummary("Get item categories")
            .RequireAuthorization();

        group.MapPatch("/{id:guid}/availability", UpdateItemAvailabilityAsync)
            .WithName("UpdateItemAvailability")
            .WithSummary("Update item availability")
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> GetItemsAsync(
        IItemService itemService,
        ClaimsPrincipal user,
        [FromQuery] Guid? communityId = null)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = communityId.HasValue
            ? await itemService.GetByCommunityIdAsync(communityId.Value, userId)
            : await itemService.GetUserItemsAsync(userId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> GetItemByIdAsync(
        IItemService itemService,
        ClaimsPrincipal user,
        Guid id)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await itemService.GetByIdAsync(id, userId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.Error);
    }

    private static async Task<IResult> CreateItemAsync(
        IItemService itemService,
        ClaimsPrincipal user,
        [FromBody] CreateItemRequest request)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await itemService.CreateAsync(request, userId);

        return result.IsSuccess
            ? Results.Created($"/api/items/{result.Value.Id}", result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> UpdateItemAsync(
        IItemService itemService,
        ClaimsPrincipal user,
        Guid id,
        [FromBody] UpdateItemRequest request)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await itemService.UpdateAsync(id, request, userId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> DeleteItemAsync(
        IItemService itemService,
        ClaimsPrincipal user,
        Guid id)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await itemService.DeleteAsync(id, userId);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> SearchItemsAsync(
        IItemService itemService,
        ClaimsPrincipal user,
        [AsParameters] ItemSearchRequest request)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await itemService.SearchAsync(request, userId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> GetCategoriesAsync(IItemService itemService)
    {
        var result = await itemService.GetCategoriesAsync();

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> UpdateItemAvailabilityAsync(
        IItemService itemService,
        ClaimsPrincipal user,
        Guid id,
        [FromBody] UpdateAvailabilityRequest request)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Results.Unauthorized();
        }

        var result = await itemService.UpdateAvailabilityAsync(id, request.Available, userId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }
}

public record UpdateAvailabilityRequest(bool Available);
