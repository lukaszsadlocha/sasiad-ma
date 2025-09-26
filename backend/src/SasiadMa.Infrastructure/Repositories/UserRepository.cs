using Microsoft.EntityFrameworkCore;
using FluentResults;
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
                ? Result.Ok(user)
                : Result.Fail($"User with id {id} was not found");
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while retrieving user");
        }
    }

    public async Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);

            return user != null
                ? Result.Ok(user)
                : Result.Fail("User with email not found");
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while retrieving user");
        }
    }

    public async Task<Result<User>> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.GoogleId == googleId, cancellationToken);

            return user != null
                ? Result.Ok(user)
                : Result.Fail("User with googleId not found");
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while retrieving user");
        }
    }

    public async Task<Result<User>> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok(user);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while creating user");
        }
    }

    public async Task<Result<User>> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok(user);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while updating user");
        }
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);
            if (user == null)
            {
                return Result.Fail($"User with id {id} was not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok(true);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while deleting user");
        }
    }

    public async Task<Result<IEnumerable<User>>> GetByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _context.Users
                .Where(u => u.CommunityMemberships.Any(cm => cm.CommunityId == communityId && cm.IsActive))
                .ToListAsync(cancellationToken);

            return Result.Ok<IEnumerable<User>>(users);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while retrieving community users");
        }
    }

    public async Task<Result<bool>> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await _context.Users
                .AnyAsync(u => u.Email.Value == email, cancellationToken);

            return Result.Ok(exists);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while checking email existence");
        }
    }

    public async Task<Result<User>> GetByEmailConfirmationTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.EmailConfirmationToken == token, cancellationToken);

            return user != null
                ? Result.Ok(user)
                : Result.Fail("User with token not found");
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while retrieving user");
        }
    }
}