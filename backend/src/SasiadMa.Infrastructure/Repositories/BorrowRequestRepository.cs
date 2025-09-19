using Microsoft.EntityFrameworkCore;
using SasiadMa.Core.Common;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Enums;
using SasiadMa.Core.Interfaces;
using SasiadMa.Infrastructure.Data;

namespace SasiadMa.Infrastructure.Repositories;

public class BorrowRequestRepository : IBorrowRequestRepository
{
    private readonly ApplicationDbContext _context;

    public BorrowRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<BorrowRequest>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = await _context.BorrowRequests
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            return request != null
                ? Result<BorrowRequest>.Success(request)
                : Result<BorrowRequest>.Failure(Error.NotFound("BorrowRequest", id.ToString()));
        }
        catch (Exception)
        {
            return Result<BorrowRequest>.Failure(Error.Unexpected("An error occurred while retrieving borrow request"));
        }
    }

    public async Task<Result<BorrowRequest>> CreateAsync(BorrowRequest borrowRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.BorrowRequests.Add(borrowRequest);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<BorrowRequest>.Success(borrowRequest);
        }
        catch (Exception)
        {
            return Result<BorrowRequest>.Failure(Error.Unexpected("An error occurred while creating borrow request"));
        }
    }

    public async Task<Result<BorrowRequest>> UpdateAsync(BorrowRequest borrowRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            borrowRequest.UpdatedAt = DateTime.UtcNow;
            _context.BorrowRequests.Update(borrowRequest);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<BorrowRequest>.Success(borrowRequest);
        }
        catch (Exception)
        {
            return Result<BorrowRequest>.Failure(Error.Unexpected("An error occurred while updating borrow request"));
        }
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var borrowRequest = await _context.BorrowRequests.FindAsync(new object[] { id }, cancellationToken);
            if (borrowRequest == null)
            {
                return Result<bool>.Failure(Error.NotFound("BorrowRequest", id.ToString()));
            }

            // Instead of hard delete, cancel the request
            borrowRequest.Status = BorrowStatus.Cancelled;
            borrowRequest.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(Error.Unexpected("An error occurred while deleting borrow request"));
        }
    }

    public async Task<Result<IEnumerable<BorrowRequest>>> GetByBorrowerIdAsync(Guid borrowerId, CancellationToken cancellationToken = default)
    {
        try
        {
            var requests = await _context.BorrowRequests
                .Where(br => br.BorrowerId == borrowerId)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<BorrowRequest>>.Success(requests);
        }
        catch (Exception)
        {
            return Result<IEnumerable<BorrowRequest>>.Failure(Error.Unexpected("An error occurred while retrieving borrow requests by borrower"));
        }
    }

    public async Task<Result<IEnumerable<BorrowRequest>>> GetByLenderIdAsync(Guid lenderId, CancellationToken cancellationToken = default)
    {
        try
        {
            var requests = await _context.BorrowRequests
                .Include(br => br.Item)
                .Where(br => br.Item.OwnerId == lenderId)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<BorrowRequest>>.Success(requests);
        }
        catch (Exception)
        {
            return Result<IEnumerable<BorrowRequest>>.Failure(Error.Unexpected("An error occurred while retrieving borrow requests by lender"));
        }
    }

    public async Task<Result<IEnumerable<BorrowRequest>>> GetByItemIdAsync(Guid itemId, CancellationToken cancellationToken = default)
    {
        try
        {
            var requests = await _context.BorrowRequests
                .Where(br => br.ItemId == itemId)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<BorrowRequest>>.Success(requests);
        }
        catch (Exception)
        {
            return Result<IEnumerable<BorrowRequest>>.Failure(Error.Unexpected("An error occurred while retrieving borrow requests by item"));
        }
    }

    public async Task<Result<IEnumerable<BorrowRequest>>> GetByStatusAsync(BorrowStatus status, CancellationToken cancellationToken = default)
    {
        try
        {
            var requests = await _context.BorrowRequests
                .Where(br => br.Status == status)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<BorrowRequest>>.Success(requests);
        }
        catch (Exception)
        {
            return Result<IEnumerable<BorrowRequest>>.Failure(Error.Unexpected("An error occurred while retrieving borrow requests by status"));
        }
    }

    public async Task<Result<IEnumerable<BorrowRequest>>> GetOverdueRequestsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var currentDate = DateTime.UtcNow;
            var requests = await _context.BorrowRequests
                .Where(br => br.Status == BorrowStatus.Active &&
                           br.RequestedEndDate < currentDate)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<BorrowRequest>>.Success(requests);
        }
        catch (Exception)
        {
            return Result<IEnumerable<BorrowRequest>>.Failure(Error.Unexpected("An error occurred while retrieving overdue borrow requests"));
        }
    }
}