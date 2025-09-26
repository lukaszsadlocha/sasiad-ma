using SasiadMa.Application.DTOs.Community;
using SasiadMa.Application.Interfaces;
using FluentResults;
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
                return Result.Fail("Operation failed");
            }

            // Check if user is a member of the community
            var isMember = await _communityRepository.IsUserMemberAsync(id, requestingUserId);
            if (!isMember.IsSuccess || !isMember.Value)
            {
                return Result.Fail("Access forbidden");
            }

            var communityDto = MapToCommunityDto(communityResult.Value);
            return Result.Ok(communityDto);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while retrieving community");
        }
    }

    public async Task<Result<IEnumerable<CommunityDto>>> GetUserCommunitiesAsync(Guid userId)
    {
        try
        {
            var communitiesResult = await _communityRepository.GetByUserIdAsync(userId);
            if (!communitiesResult.IsSuccess)
            {
                return Result.Fail<IEnumerable<CommunityDto>>("Operation failed");
            }

            var communityDtos = communitiesResult.Value.Select(MapToCommunityDto);
            return Result.Ok<IEnumerable<CommunityDto>>(communityDtos);
        }
        catch (Exception)
        {
            return Result.Fail<IEnumerable<CommunityDto>>("An error occurred while retrieving user communities");
        }
    }

    public async Task<Result<CommunityDto>> CreateAsync(CreateCommunityRequest request, Guid createdBy)
    {
        try
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Result.Fail("Community name is required");
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
                return Result.Fail("Operation failed");
            }

            // Add creator as admin member
            var memberResult = await _communityRepository.AddMemberAsync(community.Id, createdBy, isAdmin: true);
            if (!memberResult.IsSuccess)
            {
                return Result.Fail("Operation failed");
            }

            var communityDto = MapToCommunityDto(createResult.Value);
            return Result.Ok(communityDto);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while creating community");
        }
    }

    public Task<Result<CommunityDto>> UpdateAsync(Guid id, UpdateCommunityRequest request, Guid userId)
    {
        // TODO: Implement community update
        return Task.FromResult(Result.Fail<CommunityDto>("Community update not yet implemented"));
    }

    public Task<Result<bool>> DeleteAsync(Guid id, Guid userId)
    {
        // TODO: Implement community deletion
        return Task.FromResult(Result.Fail<bool>("Community deletion not yet implemented"));
    }

    public async Task<Result<bool>> JoinAsync(JoinCommunityRequest request, Guid userId)
    {
        try
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.InvitationCode))
            {
                return Result.Fail("Invitation code is required");
            }

            // Find community by invitation code
            var communityResult = await _communityRepository.GetByInvitationCodeAsync(request.InvitationCode);
            if (!communityResult.IsSuccess)
            {
                return Result.Fail("Invalid invitation code");
            }

            var community = communityResult.Value;

            // Check if community is active
            if (!community.IsActive)
            {
                return Result.Fail("Community is not active");
            }

            // Add user as member
            var memberResult = await _communityRepository.AddMemberAsync(community.Id, userId);
            if (!memberResult.IsSuccess)
            {
                return Result.Fail("Operation failed");
            }

            return Result.Ok(true);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while joining community");
        }
    }

    public Task<Result<bool>> LeaveAsync(Guid communityId, Guid userId)
    {
        // TODO: Implement community leaving
        return Task.FromResult(Result.Fail<bool>("Community leaving not yet implemented"));
    }

    public Task<Result<bool>> RemoveMemberAsync(Guid communityId, Guid memberUserId, Guid adminUserId)
    {
        // TODO: Implement member removal
        return Task.FromResult(Result.Fail<bool>("Member removal not yet implemented"));
    }

    public Task<Result<bool>> MakeMemberAdminAsync(Guid communityId, Guid memberUserId, Guid adminUserId)
    {
        // TODO: Implement making member admin
        return Task.FromResult(Result.Fail<bool>("Making member admin not yet implemented"));
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
                return Result.Fail("Access forbidden");
            }

            // Generate new invitation code
            var newCode = GenerateInvitationCode();
            var updateResult = await _communityRepository.UpdateInvitationCodeAsync(communityId, newCode);

            if (!updateResult.IsSuccess)
            {
                return Result.Fail("Operation failed");
            }

            return Result.Ok(newCode);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while generating invitation code");
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
                return Result.Fail<IEnumerable<SasiadMa.Application.DTOs.User.UserDto>>("Access forbidden");
            }

            var membersResult = await _communityRepository.GetMembersAsync(communityId);
            if (!membersResult.IsSuccess)
            {
                return Result.Fail<IEnumerable<SasiadMa.Application.DTOs.User.UserDto>>("Operation failed");
            }

            var memberDtos = membersResult.Value.Select(member => new SasiadMa.Application.DTOs.User.UserDto
            {
                Id = member.UserId,
                FirstName = member.User?.FirstName ?? "",
                LastName = member.User?.LastName ?? "",
                Email = member.User?.Email ?? "",
                ProfileImageUrl = member.User?.ProfileImageUrl
            });

            return Result.Ok<IEnumerable<SasiadMa.Application.DTOs.User.UserDto>>(memberDtos);
        }
        catch (Exception)
        {
            return Result.Fail<IEnumerable<SasiadMa.Application.DTOs.User.UserDto>>("An error occurred while retrieving community members");
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