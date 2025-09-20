using SasiadMa.Application.DTOs.Community;
using SasiadMa.Application.Interfaces;
using SasiadMa.Core.Common;
using SasiadMa.Core.Interfaces;
using System.Linq;

namespace SasiadMa.Application.Services;

public class CommunityService : ICommunityService
{
    private readonly ICommunityRepository _communityRepository;
    private readonly IUserRepository _userRepository;

    public CommunityService(ICommunityRepository communityRepository, IUserRepository userRepository)
    {
        _communityRepository = communityRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<CommunityDto>> GetByIdAsync(Guid id, Guid requestingUserId)
    {
        try
        {
            var communityResult = await _communityRepository.GetByIdAsync(id);
            if (!communityResult.IsSuccess)
            {
                return Result<CommunityDto>.Failure(communityResult.Error);
            }

            // Check if user is a member of the community
            var isMember = await _communityRepository.IsUserMemberAsync(id, requestingUserId);
            if (!isMember.IsSuccess || !isMember.Value)
            {
                return Result<CommunityDto>.Failure(Error.Forbidden("You are not a member of this community"));
            }

            var communityDto = MapToCommunityDto(communityResult.Value);
            return Result<CommunityDto>.Success(communityDto);
        }
        catch (Exception)
        {
            return Result<CommunityDto>.Failure(Error.Unexpected("An error occurred while retrieving community"));
        }
    }

    public async Task<Result<IEnumerable<CommunityDto>>> GetUserCommunitiesAsync(Guid userId)
    {
        try
        {
            var communitiesResult = await _communityRepository.GetByUserIdAsync(userId);
            if (!communitiesResult.IsSuccess)
            {
                return Result<IEnumerable<CommunityDto>>.Failure(communitiesResult.Error);
            }

            var communityDtos = communitiesResult.Value.Select(MapToCommunityDto);
            return Result<IEnumerable<CommunityDto>>.Success(communityDtos);
        }
        catch (Exception)
        {
            return Result<IEnumerable<CommunityDto>>.Failure(Error.Unexpected("An error occurred while retrieving user communities"));
        }
    }

    public async Task<Result<CommunityDto>> CreateAsync(CreateCommunityRequest request, Guid createdBy)
    {
        try
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Result<CommunityDto>.Failure(Error.Validation("name", "Community name is required"));
            }

            // Create community entity
            var community = new Core.Entities.Community
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Description = request.Description?.Trim() ?? string.Empty,
                ImageUrl = request.ImageUrl,
                IsPublic = request.IsPublic,
                MaxMembers = request.MaxMembers,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Generate invitation code
            community.GenerateNewInvitationCode();

            // Create community in repository
            var createResult = await _communityRepository.CreateAsync(community);
            if (!createResult.IsSuccess)
            {
                return Result<CommunityDto>.Failure(createResult.Error);
            }

            // Add creator as admin member
            var memberResult = await _communityRepository.AddMemberAsync(community.Id, createdBy, isAdmin: true);
            if (!memberResult.IsSuccess)
            {
                return Result<CommunityDto>.Failure(memberResult.Error);
            }

            var communityDto = MapToCommunityDto(createResult.Value);
            return Result<CommunityDto>.Success(communityDto);
        }
        catch (Exception)
        {
            return Result<CommunityDto>.Failure(Error.Unexpected("An error occurred while creating community"));
        }
    }

    public Task<Result<CommunityDto>> UpdateAsync(Guid id, UpdateCommunityRequest request, Guid userId)
    {
        // TODO: Implement community update
        return Task.FromResult(Result<CommunityDto>.Failure(Error.Validation("feature", "Community update not yet implemented")));
    }

    public Task<Result<bool>> DeleteAsync(Guid id, Guid userId)
    {
        // TODO: Implement community deletion
        return Task.FromResult(Result<bool>.Failure(Error.Validation("feature", "Community deletion not yet implemented")));
    }

    public async Task<Result<bool>> JoinAsync(JoinCommunityRequest request, Guid userId)
    {
        try
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.InvitationCode))
            {
                return Result<bool>.Failure(Error.Validation("invitationCode", "Invitation code is required"));
            }

            // Find community by invitation code
            var communityResult = await _communityRepository.GetByInvitationCodeAsync(request.InvitationCode);
            if (!communityResult.IsSuccess)
            {
                return Result<bool>.Failure(Error.Validation("invitationCode", "Invalid invitation code"));
            }

            var community = communityResult.Value;

            // Check if community is active
            if (!community.IsActive)
            {
                return Result<bool>.Failure(Error.Validation("community", "Community is not active"));
            }

            // Add user as member
            var memberResult = await _communityRepository.AddMemberAsync(community.Id, userId);
            if (!memberResult.IsSuccess)
            {
                return Result<bool>.Failure(memberResult.Error);
            }

            return Result<bool>.Success(true);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(Error.Unexpected("An error occurred while joining community"));
        }
    }

    public Task<Result<bool>> LeaveAsync(Guid communityId, Guid userId)
    {
        // TODO: Implement community leaving
        return Task.FromResult(Result<bool>.Failure(Error.Validation("feature", "Community leaving not yet implemented")));
    }

    public Task<Result<bool>> RemoveMemberAsync(Guid communityId, Guid memberUserId, Guid adminUserId)
    {
        // TODO: Implement member removal
        return Task.FromResult(Result<bool>.Failure(Error.Validation("feature", "Member removal not yet implemented")));
    }

    public Task<Result<bool>> MakeMemberAdminAsync(Guid communityId, Guid memberUserId, Guid adminUserId)
    {
        // TODO: Implement making member admin
        return Task.FromResult(Result<bool>.Failure(Error.Validation("feature", "Making member admin not yet implemented")));
    }

    private CommunityDto MapToCommunityDto(Core.Entities.Community community)
    {
        return new CommunityDto
        {
            Id = community.Id,
            Name = community.Name,
            Description = community.Description,
            ImageUrl = community.ImageUrl,
            InvitationCode = community.InvitationCode,
            Address = community.Address,
            City = community.City,
            PostalCode = community.PostalCode,
            Latitude = community.Latitude,
            Longitude = community.Longitude,
            IsPublic = community.IsPublic,
            IsActive = community.IsActive,
            MaxMembers = community.MaxMembers,
            ActiveMembersCount = 0, // TODO: Calculate actual count
            CanAcceptNewMembers = true, // TODO: Calculate based on max members
            CreatedAt = community.CreatedAt,
            Members = new List<CommunityMemberDto>()
        };
    }

    public async Task<Result<string>> GenerateInvitationCodeAsync(Guid communityId, Guid userId)
    {
        try
        {
            // Check if user is an admin of the community
            var isAdminResult = await _communityRepository.IsUserAdminAsync(communityId, userId);
            if (!isAdminResult.IsSuccess || !isAdminResult.Value)
            {
                return Result<string>.Failure(Error.Forbidden("Only admins can generate invitation codes"));
            }

            // Generate new invitation code
            var newCode = GenerateInvitationCode();
            var updateResult = await _communityRepository.UpdateInvitationCodeAsync(communityId, newCode);

            if (!updateResult.IsSuccess)
            {
                return Result<string>.Failure(updateResult.Error);
            }

            return Result<string>.Success(newCode);
        }
        catch (Exception)
        {
            return Result<string>.Failure(Error.Unexpected("An error occurred while generating invitation code"));
        }
    }

    public async Task<Result<IEnumerable<SasiadMa.Application.DTOs.User.UserDto>>> GetMembersAsync(Guid communityId, Guid requestingUserId)
    {
        try
        {
            // Check if user is a member of the community
            var isMemberResult = await _communityRepository.IsUserMemberAsync(communityId, requestingUserId);
            if (!isMemberResult.IsSuccess || !isMemberResult.Value)
            {
                return Result<IEnumerable<SasiadMa.Application.DTOs.User.UserDto>>.Failure(Error.Forbidden("You are not a member of this community"));
            }

            var membersResult = await _communityRepository.GetMembersAsync(communityId);
            if (!membersResult.IsSuccess)
            {
                return Result<IEnumerable<SasiadMa.Application.DTOs.User.UserDto>>.Failure(membersResult.Error);
            }

            var memberDtos = membersResult.Value.Select(member => new SasiadMa.Application.DTOs.User.UserDto
            {
                Id = member.UserId,
                FirstName = member.User?.FirstName ?? "",
                LastName = member.User?.LastName ?? "",
                Email = member.User?.Email ?? "",
                ProfileImageUrl = member.User?.ProfileImageUrl
            });

            return Result<IEnumerable<SasiadMa.Application.DTOs.User.UserDto>>.Success(memberDtos);
        }
        catch (Exception)
        {
            return Result<IEnumerable<SasiadMa.Application.DTOs.User.UserDto>>.Failure(Error.Unexpected("An error occurred while retrieving community members"));
        }
    }

    private static string GenerateInvitationCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}