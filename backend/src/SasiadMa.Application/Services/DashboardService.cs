using SasiadMa.Application.DTOs.Dashboard;
using SasiadMa.Application.Interfaces;
using SasiadMa.Core.Common;
using SasiadMa.Core.Interfaces;

namespace SasiadMa.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly ICommunityRepository _communityRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IBorrowRequestRepository _borrowRequestRepository;
    private readonly INotificationRepository _notificationRepository;

    public DashboardService(
        ICommunityRepository communityRepository,
        IItemRepository itemRepository,
        IBorrowRequestRepository borrowRequestRepository,
        INotificationRepository notificationRepository)
    {
        _communityRepository = communityRepository;
        _itemRepository = itemRepository;
        _borrowRequestRepository = borrowRequestRepository;
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<DashboardStatsDto>> GetUserDashboardStatsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get communities count
            var communitiesResult = await _communityRepository.GetByUserIdAsync(userId, cancellationToken);
            var communitiesCount = communitiesResult.IsSuccess ? communitiesResult.Value.Count() : 0;

            // Get items count (items owned by user)
            var itemsResult = await _itemRepository.GetByOwnerIdAsync(userId, cancellationToken);
            var itemsCount = itemsResult.IsSuccess ? itemsResult.Value.Count(i => i.IsActive) : 0;

            // Get active borrows count (items currently borrowed by this user)
            var borrowRequestsResult = await _borrowRequestRepository.GetByBorrowerIdAsync(userId, cancellationToken);
            var activeBorrowsCount = borrowRequestsResult.IsSuccess
                ? borrowRequestsResult.Value.Count(br => br.Status == Core.Enums.BorrowStatus.Active)
                : 0;

            // Get unread notifications count
            var unreadNotificationsResult = await _notificationRepository.GetUnreadByUserIdAsync(userId, cancellationToken);
            var unreadNotificationsCount = unreadNotificationsResult.IsSuccess ? unreadNotificationsResult.Value.Count() : 0;

            var stats = new DashboardStatsDto
            {
                CommunitiesCount = communitiesCount,
                ItemsCount = itemsCount,
                ActiveBorrowsCount = activeBorrowsCount,
                UnreadNotificationsCount = unreadNotificationsCount
            };

            return Result<DashboardStatsDto>.Success(stats);
        }
        catch (Exception)
        {
            return Result<DashboardStatsDto>.Failure(Error.Unexpected("An error occurred while retrieving dashboard statistics"));
        }
    }
}