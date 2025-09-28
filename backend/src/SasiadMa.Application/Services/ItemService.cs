using SasiadMa.Application.DTOs.Item;
using SasiadMa.Application.Interfaces;
using FluentResults;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Enums;
using SasiadMa.Core.Interfaces;
using SasiadMa.Core.ValueObjects;

namespace SasiadMa.Application.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly ICommunityRepository _communityRepository;

    public ItemService(IItemRepository itemRepository, ICommunityRepository communityRepository)
    {
        _itemRepository = itemRepository;
        _communityRepository = communityRepository;
    }

    public async Task<Result<ItemDto>> GetByIdAsync(Guid id, Guid userId)
    {
        try
        {
            var itemResult = await _itemRepository.GetByIdAsync(id);
            if (!itemResult.IsSuccess)
            {
                return Result.Fail("Operation failed");
            }

            var item = itemResult.Value;

            // Check if user has access to this item (must be member of the community)
            var isMemberResult = await _communityRepository.IsUserMemberAsync(item.CommunityId, userId);
            if (!isMemberResult.IsSuccess || !isMemberResult.Value)
            {
                return Result.Fail("Access forbidden");
            }

            var itemDto = MapToItemDto(item);
            return Result.Ok(itemDto);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while retrieving the item");
        }
    }

    public async Task<Result<IEnumerable<ItemDto>>> GetByCommunityIdAsync(Guid communityId, Guid userId)
    {
        try
        {
            // Check if user is a member of the community
            var isMemberResult = await _communityRepository.IsUserMemberAsync(communityId, userId);
            if (!isMemberResult.IsSuccess || !isMemberResult.Value)
            {
                return Result.Fail<IEnumerable<ItemDto>>("Access forbidden");
            }

            var itemsResult = await _itemRepository.GetByCommunityIdAsync(communityId);
            if (!itemsResult.IsSuccess)
            {
                return Result.Fail<IEnumerable<ItemDto>>("Operation failed");
            }

            var itemDtos = itemsResult.Value.Select(MapToItemDto);
            return Result.Ok<IEnumerable<ItemDto>>(itemDtos);
        }
        catch (Exception)
        {
            return Result.Fail<IEnumerable<ItemDto>>("An error occurred while retrieving community items");
        }
    }

    public async Task<Result<IEnumerable<ItemDto>>> GetUserItemsAsync(Guid userId)
    {
        try
        {
            var itemsResult = await _itemRepository.GetByOwnerIdAsync(userId);
            if (!itemsResult.IsSuccess)
            {
                return Result.Fail<IEnumerable<ItemDto>>("Operation failed");
            }

            var itemDtos = itemsResult.Value.Select(MapToItemDto);
            return Result.Ok<IEnumerable<ItemDto>>(itemDtos);
        }
        catch (Exception)
        {
            return Result.Fail<IEnumerable<ItemDto>>("An error occurred while retrieving user items");
        }
    }

    public async Task<Result<IEnumerable<ItemDto>>> SearchAsync(ItemSearchRequest request, Guid userId)
    {
        try
        {
            // If community ID is specified, check if user is a member
            if (request.CommunityId.HasValue)
            {
                var isMemberResult = await _communityRepository.IsUserMemberAsync(request.CommunityId.Value, userId);
                if (!isMemberResult.IsSuccess || !isMemberResult.Value)
                {
                    return Result.Fail<IEnumerable<ItemDto>>("Access forbidden");
                }
            }

            var searchResult = await _itemRepository.SearchAsync(
                request.SearchTerm ?? string.Empty,
                request.CommunityId,
                request.Category);

            if (!searchResult.IsSuccess)
            {
                return Result.Fail<IEnumerable<ItemDto>>("Operation failed");
            }

            var items = searchResult.Value;

            // Filter by status if specified
            if (!string.IsNullOrEmpty(request.Status))
            {
                items = items.Where(i => i.Status.ToString().Equals(request.Status, StringComparison.OrdinalIgnoreCase));
            }

            // Filter to available only if requested
            if (request.AvailableOnly)
            {
                items = items.Where(i => i.Status == ItemStatus.Available);
            }

            var itemDtos = items.Select(MapToItemDto);
            return Result.Ok<IEnumerable<ItemDto>>(itemDtos);
        }
        catch (Exception)
        {
            return Result.Fail<IEnumerable<ItemDto>>("An error occurred while searching items");
        }
    }

    public async Task<Result<ItemDto>> CreateAsync(CreateItemRequest request, Guid ownerId)
    {
        try
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Result.Fail("Item name is required");
            }

            // Check if user is a member of the community
            var isMemberResult = await _communityRepository.IsUserMemberAsync(request.CommunityId, ownerId);
            if (!isMemberResult.IsSuccess || !isMemberResult.Value)
            {
                return Result.Fail("Access forbidden");
            }

            // Parse category and condition
            var category = ItemCategory.GetByCode(request.Category) ?? ItemCategory.Other;
            var condition = ItemCondition.GetByCode(request.Condition) ?? ItemCondition.Good;

            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Description = request.Description?.Trim() ?? string.Empty,
                Category = category,
                Condition = condition,
                Status = ItemStatus.Available,
                OwnerId = ownerId,
                CommunityId = request.CommunityId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Create item images if provided
            if (request.ImageUrls?.Any() == true)
            {
                var itemImages = new List<ItemImage>();
                for (int i = 0; i < request.ImageUrls.Count; i++)
                {
                    var imageUrl = request.ImageUrls[i];
                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        var itemImage = new ItemImage
                        {
                            Id = Guid.NewGuid(),
                            ItemId = item.Id,
                            Url = imageUrl.Trim(),
                            Alt = $"Image of {item.Name}",
                            IsPrimary = i == 0, // First image is primary
                            Order = i + 1,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        itemImages.Add(itemImage);
                    }
                }
                item.Images = itemImages;
            }

            var createResult = await _itemRepository.CreateAsync(item);
            if (!createResult.IsSuccess)
            {
                return Result.Fail("Operation failed");
            }

            var itemDto = MapToItemDto(createResult.Value);
            return Result.Ok(itemDto);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while creating the item");
        }
    }

    public async Task<Result<ItemDto>> UpdateAsync(Guid id, UpdateItemRequest request, Guid userId)
    {
        try
        {
            var itemResult = await _itemRepository.GetByIdAsync(id);
            if (!itemResult.IsSuccess)
            {
                return Result.Fail("Operation failed");
            }

            var item = itemResult.Value;

            // Check if user is the owner
            if (item.OwnerId != userId)
            {
                return Result.Fail("Access forbidden");
            }

            // Update fields if provided
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                item.Name = request.Name.Trim();
            }

            if (request.Description != null)
            {
                item.Description = request.Description.Trim();
            }

            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                var category = ItemCategory.GetByCode(request.Category);
                if (category != null)
                {
                    item.Category = category;
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Condition))
            {
                var condition = ItemCondition.GetByCode(request.Condition);
                if (condition != null)
                {
                    item.Condition = condition;
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<ItemStatus>(request.Status, true, out var status))
            {
                item.Status = status;
            }

            item.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _itemRepository.UpdateAsync(item);
            if (!updateResult.IsSuccess)
            {
                return Result.Fail("Operation failed");
            }

            var itemDto = MapToItemDto(updateResult.Value);
            return Result.Ok(itemDto);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while updating the item");
        }
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, Guid userId)
    {
        try
        {
            var itemResult = await _itemRepository.GetByIdAsync(id);
            if (!itemResult.IsSuccess)
            {
                return Result.Fail("Operation failed");
            }

            var item = itemResult.Value;

            // Check if user is the owner
            if (item.OwnerId != userId)
            {
                return Result.Fail("Access forbidden");
            }

            var deleteResult = await _itemRepository.DeleteAsync(id);
            return deleteResult;
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while deleting the item");
        }
    }

    public async Task<Result<ItemDto>> UpdateAvailabilityAsync(Guid id, bool available, Guid userId)
    {
        try
        {
            var itemResult = await _itemRepository.GetByIdAsync(id);
            if (!itemResult.IsSuccess)
            {
                return Result.Fail("Operation failed");
            }

            var item = itemResult.Value;

            // Check if user is the owner
            if (item.OwnerId != userId)
            {
                return Result.Fail("Access forbidden");
            }

            item.Status = available ? ItemStatus.Available : ItemStatus.Unavailable;
            item.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _itemRepository.UpdateAsync(item);
            if (!updateResult.IsSuccess)
            {
                return Result.Fail("Operation failed");
            }

            var itemDto = MapToItemDto(updateResult.Value);
            return Result.Ok(itemDto);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while updating item availability");
        }
    }

    public async Task<Result<IEnumerable<string>>> GetCategoriesAsync()
    {
        try
        {
            await Task.CompletedTask; // To make this async method compile
            var categories = ItemCategory.GetAll().Select(c => c.Code);
            return Result.Ok<IEnumerable<string>>(categories);
        }
        catch (Exception)
        {
            return Result.Fail<IEnumerable<string>>("An error occurred while retrieving categories");
        }
    }

    private ItemDto MapToItemDto(Item item)
    {
        return new ItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Category = item.Category?.Name ?? "Other",
            Condition = item.Condition?.Name ?? "Good",
            Status = item.Status.ToString(),
            ImageUrls = item.Images?.Select(i => i.Url).ToList() ?? new List<string>(),
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
            OwnerId = item.OwnerId,
            OwnerName = item.Owner != null ? $"{item.Owner.FirstName} {item.Owner.LastName}".Trim() : "Unknown",
            OwnerProfileImageUrl = item.Owner?.ProfileImageUrl,
            CommunityId = item.CommunityId,
            CommunityName = item.Community?.Name ?? "Unknown Community",
            CurrentBorrowRequestId = null, // TODO: Add logic for current borrow request
            CurrentBorrowerName = null,
            BorrowedAt = null,
            DueDate = null
        };
    }
}