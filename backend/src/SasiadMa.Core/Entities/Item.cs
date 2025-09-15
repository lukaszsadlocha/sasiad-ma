using SasiadMa.Core.Common;
using SasiadMa.Core.Enums;
using SasiadMa.Core.ValueObjects;

namespace SasiadMa.Core.Entities;

public class Item : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ItemCategory Category { get; set; } = ItemCategory.Other;
    public ItemCondition Condition { get; set; } = ItemCondition.Good;
    public ItemStatus Status { get; set; } = ItemStatus.Available;
    public decimal? EstimatedValue { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int? YearPurchased { get; set; }
    public string? UsageInstructions { get; set; }
    public bool RequiresDeposit { get; set; } = false;
    public decimal? DepositAmount { get; set; }
    public int MaxBorrowDays { get; set; } = 7;
    public bool IsActive { get; set; } = true;

    // Relationships
    public Guid OwnerId { get; set; }
    public Guid CommunityId { get; set; }

    // Navigation properties
    public virtual User Owner { get; set; } = null!;
    public virtual Community Community { get; set; } = null!;
    public virtual ICollection<ItemImage> Images { get; set; } = new List<ItemImage>();
    public virtual ICollection<BorrowRequest> BorrowRequests { get; set; } = new List<BorrowRequest>();

    public bool IsAvailableForBorrow()
    {
        return Status == ItemStatus.Available && IsActive;
    }

    public bool CanBeRequestedBy(Guid userId)
    {
        return IsAvailableForBorrow() && OwnerId != userId;
    }

    public ItemImage? GetPrimaryImage()
    {
        return Images.FirstOrDefault(i => i.IsPrimary) ?? Images.FirstOrDefault();
    }

    public void MarkAsBorrowed()
    {
        Status = ItemStatus.Borrowed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsAvailable()
    {
        Status = ItemStatus.Available;
        UpdatedAt = DateTime.UtcNow;
    }
}

public class ItemImage : BaseEntity
{
    public string Url { get; set; } = string.Empty;
    public string? Alt { get; set; }
    public bool IsPrimary { get; set; } = false;
    public int Order { get; set; } = 0;
    public string? CloudinaryPublicId { get; set; }

    // Relationships
    public Guid ItemId { get; set; }

    // Navigation properties
    public virtual Item Item { get; set; } = null!;
}
