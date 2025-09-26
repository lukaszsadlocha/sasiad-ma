using FluentResults;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Enums;

namespace SasiadMa.Core.Interfaces;

public interface INotificationRepository
{
    Task<Result<Notification>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Notification>> CreateAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<Result<Notification>> UpdateAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Notification>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Notification>>> GetUnreadByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Notification>>> GetByTypeAsync(NotificationType type, CancellationToken cancellationToken = default);
    Task<Result<bool>> MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default);
    Task<Result<bool>> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);
}
