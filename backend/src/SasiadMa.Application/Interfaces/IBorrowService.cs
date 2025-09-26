using SasiadMa.Application.DTOs.Borrow;
using FluentResults;

namespace SasiadMa.Application.Interfaces;

public interface IBorrowService
{
    Task<Result<BorrowRequestDto>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<BorrowRequestDto>>> GetUserBorrowRequestsAsync(Guid userId);
    Task<Result<IEnumerable<BorrowRequestDto>>> GetUserLendRequestsAsync(Guid userId);
    Task<Result<BorrowRequestDto>> CreateBorrowRequestAsync(CreateBorrowRequest request, Guid borrowerId);
    Task<Result<bool>> ApproveBorrowRequestAsync(Guid id, Guid lenderId);
    Task<Result<bool>> RejectBorrowRequestAsync(Guid id, Guid lenderId);
    Task<Result<bool>> MarkAsReturnedAsync(Guid id, Guid lenderId);
    Task<Result<bool>> CancelBorrowRequestAsync(Guid id, Guid borrowerId);
}
