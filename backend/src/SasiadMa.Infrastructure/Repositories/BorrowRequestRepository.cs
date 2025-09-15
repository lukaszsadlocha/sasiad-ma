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

    public Task<Result<BorrowRequest>> UpdateAsync(BorrowRequest borrowRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<BorrowRequest>>> GetByBorrowerIdAsync(Guid borrowerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<BorrowRequest>>> GetByLenderIdAsync(Guid lenderId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<BorrowRequest>>> GetByItemIdAsync(Guid itemId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<BorrowRequest>>> GetByStatusAsync(BorrowStatus status, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<BorrowRequest>>> GetOverdueRequestsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}