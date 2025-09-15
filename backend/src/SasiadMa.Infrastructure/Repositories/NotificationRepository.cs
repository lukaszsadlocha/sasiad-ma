using Microsoft.EntityFrameworkCore;
using SasiadMa.Core.Common;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Enums;
using SasiadMa.Core.Interfaces;
using SasiadMa.Infrastructure.Data;

namespace SasiadMa.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Notification>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

            return notification != null
                ? Result<Notification>.Success(notification)
                : Result<Notification>.Failure(Error.NotFound("Notification", id.ToString()));
        }
        catch (Exception)
        {
            return Result<Notification>.Failure(Error.Unexpected("An error occurred while retrieving notification"));
        }
    }

    public async Task<Result<Notification>> CreateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<Notification>.Success(notification);
        }
        catch (Exception)
        {
            return Result<Notification>.Failure(Error.Unexpected("An error occurred while creating notification"));
        }
    }

    public Task<Result<Notification>> UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Notification>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Notification>>> GetUnreadByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Notification>>> GetByTypeAsync(NotificationType type, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}