using SasiadMa.Application.DTOs.User;
using FluentResults;

namespace SasiadMa.Application.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(Guid id);
    Task<Result<UserDto>> GetByEmailAsync(string email);
    Task<Result<UserDto>> UpdateAsync(Guid id, UpdateUserRequest request);
    Task<Result<bool>> DeleteAsync(Guid id);
    Task<Result<IEnumerable<UserDto>>> GetCommunityMembersAsync(Guid communityId);
}
