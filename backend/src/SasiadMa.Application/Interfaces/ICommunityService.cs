using SasiadMa.Application.DTOs.Community;
using SasiadMa.Application.DTOs.User;
using FluentResults;

namespace SasiadMa.Application.Interfaces;

public interface ICommunityService
{
    Task<Result<CommunityDto>> GetByIdAsync(Guid id, Guid requestingUserId);
    Task<Result<IEnumerable<CommunityDto>>> GetUserCommunitiesAsync(Guid userId);
    Task<Result<CommunityDto>> CreateAsync(CreateCommunityRequest request, Guid createdBy);
    Task<Result<CommunityDto>> UpdateAsync(Guid id, UpdateCommunityRequest request, Guid userId);
    Task<Result<bool>> DeleteAsync(Guid id, Guid userId);
    Task<Result<bool>> JoinAsync(JoinCommunityRequest request, Guid userId);
    Task<Result<bool>> LeaveAsync(Guid communityId, Guid userId);
    Task<Result<bool>> RemoveMemberAsync(Guid communityId, Guid memberUserId, Guid adminUserId);
    Task<Result<bool>> MakeMemberAdminAsync(Guid communityId, Guid memberUserId, Guid adminUserId);
    Task<Result<string>> GenerateInvitationCodeAsync(Guid communityId, Guid userId);
    Task<Result<IEnumerable<UserDto>>> GetMembersAsync(Guid communityId, Guid requestingUserId);
}
