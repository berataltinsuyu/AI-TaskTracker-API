using AITaskTracker.API.DTOs;
using AITaskTracker.API.Repositories;

namespace AITaskTracker.API.Services;

public class DashboardService : IDashboardService
{
  private readonly ITaskRepository _taskRepository;
  private readonly IDailyLogRepository _dailyLogRepository;
  private readonly ILearningTopicRepository _learningTopicRepository;

  public DashboardService(
    ITaskRepository taskRepository,
    IDailyLogRepository dailyLogRepository,
    ILearningTopicRepository learningTopicRepository)
  {
    _taskRepository = taskRepository;
    _dailyLogRepository = dailyLogRepository;
    _learningTopicRepository = learningTopicRepository;
  }

  public async Task<DashboardSummaryDto> GetSummaryAsync(int userId)
  {
    var tasks = await _taskRepository.GetAllByUserIdAsync(userId);
    var dailyLogs = await _dailyLogRepository.GetAllByUserIdAsync(userId);
    var learningTopics = await _learningTopicRepository.GetAllByUserIdAsync(userId);

    var completedTasks = tasks.Count(task => task.IsCompleted);
    var totalTasks = tasks.Count;
    var pendingTasks = totalTasks - completedTasks;

    var learningTopicsByCategory = learningTopics
       .GroupBy(topic => topic.Category)
       .ToDictionary(
          group => group.Key,
          group => group.Count()
       );

    return new DashboardSummaryDto
    {
      TotalTasks = totalTasks,
      CompletedTasks = completedTasks,
      PendingTasks = pendingTasks,
      TotalDailyLogs = dailyLogs.Count,
      TotalLearningTopics = learningTopics.Count,
      LearningTopicsByCategory = learningTopicsByCategory
    };

  }
}