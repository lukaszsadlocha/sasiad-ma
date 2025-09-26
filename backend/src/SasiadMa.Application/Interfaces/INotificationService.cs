using SasiadMa.Application.DTOs.Notification;
using FluentResults;

namespace SasiadMa.Application.Interfaces;

public interface INotificationService
{
    Task<Result<IEnumerable<NotificationDto>>> GetUserNotificationsAsync(Guid userId);
    Task<Result<int>> GetUnreadCountAsync(Guid userId);
    Task<Result<bool>> MarkAsReadAsync(Guid notificationId, Guid userId);
    Task<Result<bool>> MarkAllAsReadAsync(Guid userId);
    Task<Result<NotificationDto>> CreateNotificationAsync(CreateNotificationRequest request);
}
