using SasiadMa.Core.Common;
using FluentResults;
using SasiadMa.Core.ValueObjects;

namespace SasiadMa.Core.Entities;

public class ItemRequest : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ItemCategory Category { get; set; } = ItemCategory.Other;
    public DateTime NeededBy { get; set; }
    public int MaxBorrowDays { get; set; } = 7;
    public bool IsUrgent { get; set; } = false;
    public bool IsFulfilled { get; set; } = false;
    public DateTime? FulfilledAt { get; set; }
    public Guid? FulfilledByItemId { get; set; }
    public bool IsActive { get; set; } = true;

    // Relationships
    public Guid RequesterId { get; set; }
    public Guid CommunityId { get; set; }

    // Navigation properties
    public virtual User Requester { get; set; } = null!;
    public virtual Community Community { get; set; } = null!;
    public virtual Item? FulfilledByItem { get; set; }

    public bool CanBeFulfilled()
    {
        return !IsFulfilled && IsActive && DateTime.UtcNow <= NeededBy;
    }

    public void MarkAsFulfilled(Guid itemId)
    {
        IsFulfilled = true;
        FulfilledAt = DateTime.UtcNow;
        FulfilledByItemId = itemId;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > NeededBy;
    }

    public int GetDaysUntilNeeded()
    {
        var days = (NeededBy - DateTime.UtcNow).Days;
        return Math.Max(0, days);
    }
}
