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

    public Task<Result<IEnumerable<Community>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Community>>> GetPublicCommunitiesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> ExistsByInvitationCodeAsync(string invitationCode, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<CommunityMember>> AddMemberAsync(Guid communityId, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RemoveMemberAsync(Guid communityId, Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}