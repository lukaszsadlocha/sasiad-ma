using SasiadMa.Core.Common;

namespace SasiadMa.Core.Entities;

public class Community : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string InvitationCode { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool IsPublic { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public int MaxMembers { get; set; } = 1000;

    // Navigation properties
    public virtual ICollection<CommunityMember> Members { get; set; } = new List<CommunityMember>();
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    public virtual ICollection<ItemRequest> ItemRequests { get; set; } = new List<ItemRequest>();

    public int ActiveMembersCount => Members.Count(m => m.IsActive);

    public bool CanAcceptNewMembers => ActiveMembersCount < MaxMembers;

    public IEnumerable<User> GetAdmins()
    {
        return Members
            .Where(m => m.IsActive && m.IsAdmin)
            .Select(m => m.User);
    }

    public bool HasAdmin(Guid userId)
    {
        return Members.Any(m => m.UserId == userId && m.IsActive && m.IsAdmin);
    }

    public void GenerateNewInvitationCode()
    {
        InvitationCode = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
    }
}

public class CommunityMember : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid CommunityId { get; set; }
    public bool IsAdmin { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LeftAt { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Community Community { get; set; } = null!;
}
