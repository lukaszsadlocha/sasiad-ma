using SasiadMa.Core.Common;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Enums;

namespace SasiadMa.Core.Interfaces;

public interface IBorrowRequestRepository
{
    Task<Result<BorrowRequest>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<BorrowRequest>> CreateAsync(BorrowRequest borrowRequest, CancellationToken cancellationToken = default);
    Task<Result<BorrowRequest>> UpdateAsync(BorrowRequest borrowRequest, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<BorrowRequest>>> GetByBorrowerIdAsync(Guid borrowerId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<BorrowRequest>>> GetByLenderIdAsync(Guid lenderId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<BorrowRequest>>> GetByItemIdAsync(Guid itemId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<BorrowRequest>>> GetByStatusAsync(BorrowStatus status, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<BorrowRequest>>> GetOverdueRequestsAsync(CancellationToken cancellationToken = default);
}
