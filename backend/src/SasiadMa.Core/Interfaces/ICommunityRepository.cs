using FluentResults;
using SasiadMa.Core.Entities;

namespace SasiadMa.Core.Interfaces;

public interface ICommunityRepository
{
    Task<Result<Community>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Community>> GetByInvitationCodeAsync(string invitationCode, CancellationToken cancellationToken = default);
    Task<Result<Community>> CreateAsync(Community community, CancellationToken cancellationToken = default);
    Task<Result<Community>> UpdateAsync(Community community, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Community>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Community>>> GetPublicCommunitiesAsync(CancellationToken cancellationToken = default);
    Task<Result<bool>> ExistsByInvitationCodeAsync(string invitationCode, CancellationToken cancellationToken = default);
    Task<Result<CommunityMember>> AddMemberAsync(Guid communityId, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);
    Task<Result> RemoveMemberAsync(Guid communityId, Guid userId, CancellationToken cancellationToken = default);
    Task<Result<bool>> IsUserMemberAsync(Guid communityId, Guid userId, CancellationToken cancellationToken = default);
    Task<Result<bool>> IsUserAdminAsync(Guid communityId, Guid userId, CancellationToken cancellationToken = default);
    Task<Result<bool>> UpdateInvitationCodeAsync(Guid communityId, string newCode, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CommunityMember>>> GetMembersAsync(Guid communityId, CancellationToken cancellationToken = default);
}
