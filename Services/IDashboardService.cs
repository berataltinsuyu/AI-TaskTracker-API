using AITaskTracker.API.DTOs;
namespace AITaskTracker.API.Services;

public interface IDashboardService
{
  Task<DashboardSummaryDto> GetSummaryAsync();
}