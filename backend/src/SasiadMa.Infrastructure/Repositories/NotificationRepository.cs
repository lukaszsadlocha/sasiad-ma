using Microsoft.EntityFrameworkCore;
using FluentResults;
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
                ? Result.Ok(notification)
                : Result.Fail("Operation failed");
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while retrieving notification");
        }
    }

    public async Task<Result<Notification>> CreateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok(notification);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while creating notification");
        }
    }

    public async Task<Result<Notification>> UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        try
        {
            notification.UpdatedAt = DateTime.UtcNow;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok(notification);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while updating notification");
        }
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var notification = await _context.Notifications.FindAsync(new object[] { id }, cancellationToken);
            if (notification == null)
            {
                return Result.Fail("Operation failed");
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok(true);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while deleting notification");
        }
    }

    public async Task<Result<IEnumerable<Notification>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync(cancellationToken);

            return Result.Ok<IEnumerable<Notification>>(notifications);
        }
        catch (Exception)
        {
            return Result.Fail<IEnumerable<Notification>>("An error occurred while retrieving notifications by user");
        }
    }

    public async Task<Result<IEnumerable<Notification>>> GetUnreadByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync(cancellationToken);

            return Result.Ok<IEnumerable<Notification>>(notifications);
        }
        catch (Exception)
        {
            return Result.Fail<IEnumerable<Notification>>("An error occurred while retrieving unread notifications");
        }
    }

    public async Task<Result<IEnumerable<Notification>>> GetByTypeAsync(NotificationType type, CancellationToken cancellationToken = default)
    {
        try
        {
            var notifications = await _context.Notifications
                .Where(n => n.Type == type)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync(cancellationToken);

            return Result.Ok<IEnumerable<Notification>>(notifications);
        }
        catch (Exception)
        {
            return Result.Fail<IEnumerable<Notification>>("An error occurred while retrieving notifications by type");
        }
    }

    public async Task<Result<bool>> MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        try
        {
            var notification = await _context.Notifications.FindAsync(new object[] { notificationId }, cancellationToken);
            if (notification == null)
            {
                return Result.Fail("Operation failed");
            }

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            notification.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok(true);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while marking notification as read");
        }
    }

    public async Task<Result<bool>> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var unreadNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync(cancellationToken);

            var currentTime = DateTime.UtcNow;
            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                notification.ReadAt = currentTime;
                notification.UpdatedAt = currentTime;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok(true);
        }
        catch (Exception)
        {
            return Result.Fail("An error occurred while marking all notifications as read");
        }
    }
}