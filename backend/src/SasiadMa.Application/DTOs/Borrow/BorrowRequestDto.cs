namespace SasiadMa.Application.DTOs.Borrow;

public class BorrowRequestDto
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime RequestedDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? BorrowedDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    // Item info
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ItemDescription { get; set; } = string.Empty;
    public List<string> ItemImageUrls { get; set; } = new();
    
    // Borrower info
    public Guid BorrowerId { get; set; }
    public string BorrowerName { get; set; } = string.Empty;
    public string BorrowerEmail { get; set; } = string.Empty;
    public string? BorrowerProfileImageUrl { get; set; }
    public int BorrowerReputationScore { get; set; }
    
    // Lender info
    public Guid LenderId { get; set; }
    public string LenderName { get; set; } = string.Empty;
    public string LenderEmail { get; set; } = string.Empty;
    public string? LenderProfileImageUrl { get; set; }
    
    // Community info
    public Guid CommunityId { get; set; }
    public string CommunityName { get; set; } = string.Empty;
}

public class CreateBorrowRequest
{
    public Guid ItemId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime RequestedDate { get; set; }
    public int BorrowDurationDays { get; set; } = 7;
}

public class UpdateBorrowRequest
{
    public string? Message { get; set; }
    public DateTime? RequestedDate { get; set; }
    public int? BorrowDurationDays { get; set; }
}
