using SasiadMa.Core.Common;
using SasiadMa.Core.Entities;
using SasiadMa.Core.Enums;

namespace SasiadMa.Core.Interfaces;

public interface INotificationRepository
{
    Task<Result<Notification>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Notification>> CreateAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<Result<Notification>> UpdateAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<r> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Notification>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Notification>>> GetUnreadByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Notification>>> GetByTypeAsync(NotificationType type, CancellationToken cancellationToken = default);
    Task<r> MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default);
    Task<r> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);
}
