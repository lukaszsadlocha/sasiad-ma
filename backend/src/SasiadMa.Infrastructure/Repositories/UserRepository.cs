using Microsoft.EntityFrameworkCore;
using SasiadMa.Core.Common;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Interfaces;
using SasiadMa.Infrastructure.Data;

namespace SasiadMa.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            return user != null
                ? Result<User>.Success(user)
                : Result<User>.Failure(Error.NotFound("User", id.ToString()));
        }
        catch (Exception)
        {
            return Result<User>.Failure(Error.Unexpected("An error occurred while retrieving user"));
        }
    }

    public async Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);

            return user != null
                ? Result<User>.Success(user)
                : Result<User>.Failure(Error.NotFound("User", "email"));
        }
        catch (Exception)
        {
            return Result<User>.Failure(Error.Unexpected("An error occurred while retrieving user"));
        }
    }

    public async Task<Result<User>> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.GoogleId == googleId, cancellationToken);

            return user != null
                ? Result<User>.Success(user)
                : Result<User>.Failure(Error.NotFound("User", "googleId"));
        }
        catch (Exception)
        {
            return Result<User>.Failure(Error.Unexpected("An error occurred while retrieving user"));
        }
    }

    public async Task<Result<User>> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<User>.Success(user);
        }
        catch (Exception)
        {
            return Result<User>.Failure(Error.Unexpected("An error occurred while creating user"));
        }
    }

    public async Task<Result<User>> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<User>.Success(user);
        }
        catch (Exception)
        {
            return Result<User>.Failure(Error.Unexpected("An error occurred while updating user"));
        }
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);
            if (user == null)
            {
                return Result<bool>.Failure(Error.NotFound("User", id.ToString()));
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(Error.Unexpected("An error occurred while deleting user"));
        }
    }

    public async Task<Result<IEnumerable<User>>> GetByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _context.Users
                .Where(u => u.CommunityMemberships.Any(cm => cm.CommunityId == communityId && cm.IsActive))
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<User>>.Success(users);
        }
        catch (Exception)
        {
            return Result<IEnumerable<User>>.Failure(Error.Unexpected("An error occurred while retrieving community users"));
        }
    }

    public async Task<Result<bool>> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await _context.Users
                .AnyAsync(u => u.Email.Value == email, cancellationToken);

            return Result<bool>.Success(exists);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(Error.Unexpected("An error occurred while checking email existence"));
        }
    }

    public async Task<Result<User>> GetByEmailConfirmationTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.EmailConfirmationToken == token, cancellationToken);

            return user != null
                ? Result<User>.Success(user)
                : Result<User>.Failure(Error.NotFound("User", "token"));
        }
        catch (Exception)
        {
            return Result<User>.Failure(Error.Unexpected("An error occurred while retrieving user"));
        }
    }
}