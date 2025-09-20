using Microsoft.EntityFrameworkCore;
using SasiadMa.Core.Common;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Interfaces;
using SasiadMa.Infrastructure.Data;

namespace SasiadMa.Infrastructure.Repositories;

public class CommunityRepository : ICommunityRepository
{
    private readonly ApplicationDbContext _context;

    public CommunityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Community>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var community = await _context.Communities
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            return community != null
                ? Result<Community>.Success(community)
                : Result<Community>.Failure(Error.NotFound("Community", id.ToString()));
        }
        catch (Exception)
        {
            return Result<Community>.Failure(Error.Unexpected("An error occurred while retrieving community"));
        }
    }

    public async Task<Result<Community>> GetByInvitationCodeAsync(string invitationCode, CancellationToken cancellationToken = default)
    {
        try
        {
            var community = await _context.Communities
                .FirstOrDefaultAsync(c => c.InvitationCode == invitationCode, cancellationToken);

            return community != null
                ? Result<Community>.Success(community)
                : Result<Community>.Failure(Error.NotFound("Community", "invitationCode"));
        }
        catch (Exception)
        {
            return Result<Community>.Failure(Error.Unexpected("An error occurred while retrieving community"));
        }
    }

    public async Task<Result<Community>> CreateAsync(Community community, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Communities.Add(community);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<Community>.Success(community);
        }
        catch (Exception)
        {
            return Result<Community>.Failure(Error.Unexpected("An error occurred while creating community"));
        }
    }

    public Task<Result<Community>> UpdateAsync(Community community, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<Community>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var communities = await _context.Communities
                .Where(c => _context.CommunityMembers
                    .Any(cm => cm.UserId == userId && cm.CommunityId == c.Id && cm.IsActive))
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Community>>.Success(communities);
        }
        catch (Exception)
        {
            return Result<IEnumerable<Community>>.Failure(Error.Unexpected("An error occurred while retrieving user communities"));
        }
    }

    public Task<Result<IEnumerable<Community>>> GetPublicCommunitiesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> ExistsByInvitationCodeAsync(string invitationCode, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<CommunityMember>> AddMemberAsync(Guid communityId, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if user is already a member
            var existingMembership = await _context.CommunityMembers
                .FirstOrDefaultAsync(cm => cm.CommunityId == communityId && cm.UserId == userId, cancellationToken);

            if (existingMembership != null)
            {
                return Result<CommunityMember>.Failure(Error.Validation("membership", "User is already a member of this community"));
            }

            var membership = new CommunityMember
            {
                Id = Guid.NewGuid(),
                CommunityId = communityId,
                UserId = userId,
                IsAdmin = isAdmin,
                IsActive = true,
                JoinedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.CommunityMembers.Add(membership);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<CommunityMember>.Success(membership);
        }
        catch (Exception)
        {
            return Result<CommunityMember>.Failure(Error.Unexpected("An error occurred while adding member to community"));
        }
    }

    public async Task<Result> RemoveMemberAsync(Guid communityId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var membership = await _context.CommunityMembers
                .FirstOrDefaultAsync(cm => cm.CommunityId == communityId && cm.UserId == userId, cancellationToken);

            if (membership == null)
            {
                return Result.Failure(Error.NotFound("Community membership", $"{communityId}-{userId}"));
            }

            _context.CommunityMembers.Remove(membership);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(Error.Unexpected("An error occurred while removing member from community"));
        }
    }

    public async Task<Result<bool>> IsUserMemberAsync(Guid communityId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var isMember = await _context.CommunityMembers
                .AnyAsync(cm => cm.CommunityId == communityId && cm.UserId == userId && cm.IsActive, cancellationToken);

            return Result<bool>.Success(isMember);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(Error.Unexpected("An error occurred while checking membership"));
        }
    }

    public async Task<Result<bool>> IsUserAdminAsync(Guid communityId, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var isAdmin = await _context.CommunityMembers
                .AnyAsync(cm => cm.CommunityId == communityId && cm.UserId == userId && cm.IsAdmin && cm.IsActive, cancellationToken);

            return Result<bool>.Success(isAdmin);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(Error.Unexpected("An error occurred while checking admin status"));
        }
    }

    public async Task<Result<bool>> UpdateInvitationCodeAsync(Guid communityId, string newCode, CancellationToken cancellationToken = default)
    {
        try
        {
            var community = await _context.Communities.FindAsync(new object[] { communityId }, cancellationToken);
            if (community == null)
            {
                return Result<bool>.Failure(Error.NotFound("Community", communityId.ToString()));
            }

            community.InvitationCode = newCode;
            community.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(Error.Unexpected("An error occurred while updating invitation code"));
        }
    }

    public async Task<Result<IEnumerable<CommunityMember>>> GetMembersAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        try
        {
            var members = await _context.CommunityMembers
                .Include(cm => cm.User)
                .Where(cm => cm.CommunityId == communityId && cm.IsActive)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<CommunityMember>>.Success(members);
        }
        catch (Exception)
        {
            return Result<IEnumerable<CommunityMember>>.Failure(Error.Unexpected("An error occurred while retrieving community members"));
        }
    }
}