namespace AITaskTracker.API.DTOs;
public class UpdateLearningTopicDto
{
  public string Name { get; set;} = string.Empty;
  public string Category { get; set;} = string.Empty;
  public string? Notes { get; set;}
}