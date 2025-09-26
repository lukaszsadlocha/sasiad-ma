using FluentResults;
using SasiadMa.Core.Entities;

namespace SasiadMa.Core.Interfaces;

public interface IItemRequestRepository
{
    Task<Result<ItemRequest>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<ItemRequest>> CreateAsync(ItemRequest itemRequest, CancellationToken cancellationToken = default);
    Task<Result<ItemRequest>> UpdateAsync(ItemRequest itemRequest, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ItemRequest>>> GetByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ItemRequest>>> GetByRequesterIdAsync(Guid requesterId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ItemRequest>>> GetActiveRequestsAsync(Guid communityId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ItemRequest>>> GetUrgentRequestsAsync(Guid communityId, CancellationToken cancellationToken = default);
}
