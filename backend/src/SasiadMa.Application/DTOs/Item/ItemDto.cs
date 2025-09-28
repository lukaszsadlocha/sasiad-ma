namespace SasiadMa.Application.DTOs.Item;

public class ItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Owner info
    public Guid OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string? OwnerProfileImageUrl { get; set; }
    
    // Community info
    public Guid CommunityId { get; set; }
    public string CommunityName { get; set; } = string.Empty;
    
    // Current borrow info (if borrowed)
    public Guid? CurrentBorrowRequestId { get; set; }
    public string? CurrentBorrowerName { get; set; }
    public DateTime? BorrowedAt { get; set; }
    public DateTime? DueDate { get; set; }
}

public class CreateItemRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public Guid CommunityId { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}

public class UpdateItemRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Condition { get; set; }
    public string? Status { get; set; }
}

public class ItemSearchRequest
{
    public Guid? CommunityId { get; set; }
    public string? SearchTerm { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; }
    public bool AvailableOnly { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
