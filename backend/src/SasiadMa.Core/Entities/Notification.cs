using SasiadMa.Core.Common;
using FluentResults;
using SasiadMa.Core.Enums;

namespace SasiadMa.Core.Entities;

public class Notification : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
    public string? ActionUrl { get; set; }
    public string? ActionText { get; set; }
    public bool IsEmailSent { get; set; } = false;
    public DateTime? EmailSentAt { get; set; }

    // Relationships
    public Guid UserId { get; set; }
    public Guid? RelatedEntityId { get; set; } // Could be ItemId, BorrowRequestId, etc.
    public string? RelatedEntityType { get; set; } // "Item", "BorrowRequest", etc.

    // Navigation properties
    public virtual User User { get; set; } = null!;

    public void MarkAsRead()
    {
        IsRead = true;
        ReadAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkEmailAsSent()
    {
        IsEmailSent = true;
        EmailSentAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool ShouldSendEmail()
    {
        // Send email for important notifications that haven't been sent yet
        return !IsEmailSent && Type is 
            NotificationType.BorrowRequest or
            NotificationType.BorrowApproved or
            NotificationType.ItemOverdue or
            NotificationType.CommunityInvitation;
    }

    public static Notification CreateBorrowRequest(Guid userId, Guid borrowRequestId, string itemName, string borrowerName)
    {
        return new Notification
        {
            UserId = userId,
            Type = NotificationType.BorrowRequest,
            Title = "New Borrow Request",
            Message = $"{borrowerName} wants to borrow your {itemName}",
            RelatedEntityId = borrowRequestId,
            RelatedEntityType = "BorrowRequest",
            ActionUrl = $"/borrow-requests/{borrowRequestId}",
            ActionText = "View Request"
        };
    }

    public static Notification CreateBorrowApproved(Guid userId, Guid borrowRequestId, string itemName)
    {
        return new Notification
        {
            UserId = userId,
            Type = NotificationType.BorrowApproved,
            Title = "Borrow Request Approved",
            Message = $"Your request to borrow {itemName} has been approved",
            RelatedEntityId = borrowRequestId,
            RelatedEntityType = "BorrowRequest",
            ActionUrl = $"/borrow-requests/{borrowRequestId}",
            ActionText = "View Details"
        };
    }

    public static Notification CreateItemOverdue(Guid userId, Guid borrowRequestId, string itemName, int daysOverdue)
    {
        return new Notification
        {
            UserId = userId,
            Type = NotificationType.ItemOverdue,
            Title = "Item Overdue",
            Message = $"{itemName} is {daysOverdue} day(s) overdue. Please return it as soon as possible.",
            RelatedEntityId = borrowRequestId,
            RelatedEntityType = "BorrowRequest",
            ActionUrl = $"/borrow-requests/{borrowRequestId}",
            ActionText = "Mark as Returned"
        };
    }
}
