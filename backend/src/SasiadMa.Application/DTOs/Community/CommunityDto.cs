namespace SasiadMa.Application.DTOs.Community;

public class CommunityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string InvitationCode { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool IsPublic { get; set; }
    public bool IsActive { get; set; }
    public int MaxMembers { get; set; }
    public int ActiveMembersCount { get; set; }
    public bool CanAcceptNewMembers { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<CommunityMemberDto> Members { get; set; } = new();
}

public class CommunityMemberDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime JoinedAt { get; set; }
    public int ReputationScore { get; set; }
    
    public string FullName => $"{FirstName} {LastName}".Trim();
}

public class CreateCommunityRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool IsPublic { get; set; } = false;
    public int MaxMembers { get; set; } = 1000;
}

public class UpdateCommunityRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool? IsPublic { get; set; }
    public int? MaxMembers { get; set; }
    public string? ImageUrl { get; set; }
}

public class JoinCommunityRequest
{
    public string InvitationCode { get; set; } = string.Empty;
}
