namespace AITaskTracker.API.DTOs;

public class DashboardSummaryDto
{
  public int TotalTasks { get; set;}
  public int CompletedTasks { get; set;}
  public int PendingTasks { get; set;}
  public int TotalDailyLogs { get; set;}
  public int TotalLearningTopics { get; set;}
  public Dictionary<string, int> LearningTopicsByCategory { get; set;} = new();
}