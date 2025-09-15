using Microsoft.EntityFrameworkCore;
using SasiadMa.Core.Common;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Interfaces;
using SasiadMa.Infrastructure.Data;

namespace SasiadMa.Infrastructure.Repositories;

public class ItemRequestRepository : IItemRequestRepository
{
    private readonly ApplicationDbContext _context;

    public ItemRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ItemRequest>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = await _context.ItemRequests
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            return request != null
                ? Result<ItemRequest>.Success(request)
                : Result<ItemRequest>.Failure(Error.NotFound("ItemRequest", id.ToString()));
        }
        catch (Exception)
        {
            return Result<ItemRequest>.Failure(Error.Unexpected("An error occurred while retrieving item request"));
        }
    }

    public async Task<Result<ItemRequest>> CreateAsync(ItemRequest itemRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.ItemRequests.Add(itemRequest);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<ItemRequest>.Success(itemRequest);
        }
        catch (Exception)
        {
            return Result<ItemRequest>.Failure(Error.Unexpected("An error occurred while creating item request"));
        }
    }

    public async Task<Result<ItemRequest>> UpdateAsync(ItemRequest itemRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.ItemRequests.Update(itemRequest);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<ItemRequest>.Success(itemRequest);
        }
        catch (Exception)
        {
            return Result<ItemRequest>.Failure(Error.Unexpected("An error occurred while updating item request"));
        }
    }

    public Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<ItemRequest>>> GetByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<ItemRequest>>> GetByRequesterIdAsync(Guid requesterId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<ItemRequest>>> GetActiveRequestsAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<ItemRequest>>> GetUrgentRequestsAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}