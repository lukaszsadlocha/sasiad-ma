using SasiadMa.Application.DTOs.Item;
using SasiadMa.Core.Common;

namespace SasiadMa.Application.Interfaces;

public interface IItemService
{
    Task<Result<ItemDto>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<ItemDto>>> GetCommunityItemsAsync(Guid communityId);
    Task<Result<IEnumerable<ItemDto>>> GetUserItemsAsync(Guid userId);
    Task<Result<IEnumerable<ItemDto>>> SearchAsync(ItemSearchRequest request);
    Task<Result<ItemDto>> CreateAsync(CreateItemRequest request, Guid ownerId);
    Task<Result<ItemDto>> UpdateAsync(Guid id, UpdateItemRequest request, Guid userId);
    Task<Result<bool>> DeleteAsync(Guid id, Guid userId);
    Task<Result<string>> UploadImageAsync(Guid itemId, Stream imageStream, string fileName, Guid userId);
}
