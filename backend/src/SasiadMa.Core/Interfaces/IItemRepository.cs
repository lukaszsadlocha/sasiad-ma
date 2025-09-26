using FluentResults;
using SasiadMa.Core.Entities;

namespace SasiadMa.Core.Interfaces;

public interface IItemRepository
{
    Task<Result<Item>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Item>> CreateAsync(Item item, CancellationToken cancellationToken = default);
    Task<Result<Item>> UpdateAsync(Item item, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Item>>> GetByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Item>>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Item>>> SearchAsync(string searchTerm, Guid? communityId = null, string? category = null, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Item>>> GetAvailableItemsAsync(Guid communityId, CancellationToken cancellationToken = default);
}
