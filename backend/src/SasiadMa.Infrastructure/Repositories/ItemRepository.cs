using Microsoft.EntityFrameworkCore;
using SasiadMa.Core.Common;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Interfaces;
using SasiadMa.Core.ValueObjects;
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
                .Include(i => i.Owner)
                .Include(i => i.Community)
                .Include(i => i.Images)
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

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var item = await _context.Items.FindAsync(new object[] { id }, cancellationToken);
            if (item == null)
            {
                return Result<bool>.Failure(Error.NotFound("Item", id.ToString()));
            }

            // Soft delete - set IsActive to false
            item.IsActive = false;
            item.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(Error.Unexpected("An error occurred while deleting item"));
        }
    }

    public async Task<Result<IEnumerable<Item>>> GetByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        try
        {
            var items = await _context.Items
                .Include(i => i.Owner)
                .Include(i => i.Community)
                .Include(i => i.Images)
                .Where(i => i.CommunityId == communityId && i.IsActive)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Item>>.Success(items);
        }
        catch (Exception)
        {
            return Result<IEnumerable<Item>>.Failure(Error.Unexpected("An error occurred while retrieving items by community"));
        }
    }

    public async Task<Result<IEnumerable<Item>>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        try
        {
            var items = await _context.Items
                .Include(i => i.Owner)
                .Include(i => i.Community)
                .Include(i => i.Images)
                .Where(i => i.OwnerId == ownerId && i.IsActive)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Item>>.Success(items);
        }
        catch (Exception)
        {
            return Result<IEnumerable<Item>>.Failure(Error.Unexpected("An error occurred while retrieving items by owner"));
        }
    }

    public async Task<Result<IEnumerable<Item>>> SearchAsync(string searchTerm, Guid? communityId = null, string? category = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.Items
                .Include(i => i.Owner)
                .Include(i => i.Community)
                .Include(i => i.Images)
                .AsQueryable();

            // Filter by search term in name or description
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(i => i.Name.Contains(searchTerm) || i.Description.Contains(searchTerm));
            }

            // Filter by community if specified
            if (communityId.HasValue)
            {
                query = query.Where(i => i.CommunityId == communityId.Value);
            }

            // Filter by category if specified
            if (!string.IsNullOrWhiteSpace(category) && ItemCategory.TryGetByCode(category, out var itemCategory))
            {
                query = query.Where(i => i.Category == itemCategory);
            }

            // Only return active items
            query = query.Where(i => i.IsActive);

            var items = await query.ToListAsync(cancellationToken);

            return Result<IEnumerable<Item>>.Success(items);
        }
        catch (Exception)
        {
            return Result<IEnumerable<Item>>.Failure(Error.Unexpected("An error occurred while searching items"));
        }
    }

    public async Task<Result<IEnumerable<Item>>> GetAvailableItemsAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        try
        {
            var items = await _context.Items
                .Where(i => i.CommunityId == communityId &&
                           i.IsActive &&
                           i.IsAvailableForBorrow)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Item>>.Success(items);
        }
        catch (Exception)
        {
            return Result<IEnumerable<Item>>.Failure(Error.Unexpected("An error occurred while retrieving available items"));
        }
    }
}