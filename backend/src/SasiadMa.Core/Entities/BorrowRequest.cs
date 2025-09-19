using SasiadMa.Core.Common;
using SasiadMa.Core.Enums;

namespace SasiadMa.Core.Entities;

public class BorrowRequest : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid BorrowerId { get; set; }
    public Guid LenderId { get; set; }
    public DateTime RequestedStartDate { get; set; }
    public DateTime RequestedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public BorrowStatus Status { get; set; } = BorrowStatus.Pending;
    public string? Message { get; set; }
    public string? ResponseMessage { get; set; }
    public DateTime? ResponseAt { get; set; }
    public int? BorrowerRating { get; set; } // 1-5 stars from lender
    public int? LenderRating { get; set; } // 1-5 stars from borrower
    public string? BorrowerFeedback { get; set; }
    public string? LenderFeedback { get; set; }
    public bool IsDepositPaid { get; set; } = false;
    public decimal? DepositAmount { get; set; }

    // Navigation properties
    public virtual Item Item { get; set; } = null!;
    public virtual User Borrower { get; set; } = null!;
    public virtual User Lender { get; set; } = null!;

    public bool CanBeApproved()
    {
        return Status == BorrowStatus.Pending;
    }

    public bool CanBeRejected()
    {
        return Status == BorrowStatus.Pending;
    }

    public bool CanBeCancelled()
    {
        return Status is BorrowStatus.Pending or BorrowStatus.Approved;
    }

    public bool CanBeReturned()
    {
        return Status == BorrowStatus.Active;
    }
    

    public void Approve(string? responseMessage = null)
    {
        Status = BorrowStatus.Approved;
        ResponseMessage = responseMessage;
        ResponseAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reject(string? responseMessage = null)
    {
        Status = BorrowStatus.Rejected;
        ResponseMessage = responseMessage;
        ResponseAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void StartBorrow()
    {
        Status = BorrowStatus.Active;
        ActualStartDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CompleteBorrow()
    {
        Status = BorrowStatus.Returned;
        ActualEndDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = BorrowStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public int GetDurationInDays()
    {
        return (RequestedEndDate - RequestedStartDate).Days + 1;
    }
}
