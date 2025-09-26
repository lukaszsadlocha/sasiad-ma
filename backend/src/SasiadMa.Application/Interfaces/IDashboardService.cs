using SasiadMa.Application.DTOs.Dashboard;
using FluentResults;

namespace SasiadMa.Application.Interfaces;

public interface IDashboardService
{
    Task<Result<DashboardStatsDto>> GetUserDashboardStatsAsync(Guid userId, CancellationToken cancellationToken = default);
}