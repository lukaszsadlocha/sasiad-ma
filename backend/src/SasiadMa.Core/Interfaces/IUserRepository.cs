using SasiadMa.Core.Common;
using SasiadMa.Core.Entities;
using SasiadMa.Core.ValueObjects;

namespace SasiadMa.Core.Interfaces;

public interface IUserRepository
{
    Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<User>> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken = default);
    Task<Result<User>> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<Result<User>> UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<User>>> GetByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken = default);
    Task<Result<bool>> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<User>> GetByEmailConfirmationTokenAsync(string token, CancellationToken cancellationToken = default);
}
