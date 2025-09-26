using SasiadMa.Core.Common;
using FluentResults;
using SasiadMa.Core.Enums;
using SasiadMa.Core.ValueObjects;

namespace SasiadMa.Core.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Email Email { get; set; } = null!;
    public string PasswordHash { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public string? Bio { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string? EmailConfirmationToken { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public ReputationScore Reputation { get; set; } = ReputationScore.Initial();
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
    public string? GoogleId { get; set; }

    // Navigation properties
    public virtual ICollection<CommunityMember> CommunityMemberships { get; set; } = new List<CommunityMember>();
    public virtual ICollection<Item> OwnedItems { get; set; } = new List<Item>();
    public virtual ICollection<BorrowRequest> BorrowRequests { get; set; } = new List<BorrowRequest>();
    public virtual ICollection<BorrowRequest> LendRequests { get; set; } = new List<BorrowRequest>();
    public virtual ICollection<ItemRequest> ItemRequests { get; set; } = new List<ItemRequest>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public string FullName => $"{FirstName} {LastName}".Trim();

    public bool IsMemberOf(Guid communityId)
    {
        return CommunityMemberships.Any(cm => cm.CommunityId == communityId && cm.IsActive);
    }

    public bool IsAdminOf(Guid communityId)
    {
        return CommunityMemberships.Any(cm => cm.CommunityId == communityId && cm.IsActive && cm.IsAdmin);
    }
}
