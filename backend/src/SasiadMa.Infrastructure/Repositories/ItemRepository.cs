using Microsoft.EntityFrameworkCore;
using SasiadMa.Core.Common;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Interfaces;
using SasiadMa.Infrastructure.Data;

namespace SasiadMa.Infrastructure.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly ApplicationDbContext _context;

    public ItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Item>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

            return item != null
                ? Result<Item>.Success(item)
                : Result<Item>.Failure(Error.NotFound("Item", id.ToString()));
        }
        catch (Exception)
        {
            return Result<Item>.Failure(Error.Unexpected("An error occurred while retrieving item"));
        }
    }

    public async Task<Result<Item>> CreateAsync(Item item, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<Item>.Success(item);
        }
        catch (Exception)
        {
            return Result<Item>.Failure(Error.Unexpected("An error occurred while creating item"));
        }
    }

    public async Task<Result<Item>> UpdateAsync(Item item, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<Item>.Success(item);
        }
        catch (Exception)
        {
            return Result<Item>.Failure(Error.Unexpected("An error occurred while updating item"));
        }
    }

    public Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Item>>> GetByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Item>>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Item>>> SearchAsync(string searchTerm, Guid? communityId = null, string? category = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Item>>> GetAvailableItemsAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}