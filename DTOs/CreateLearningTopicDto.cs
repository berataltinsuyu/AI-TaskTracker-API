namespace AITaskTracker.API.DTOs;
public class CreateLearningTopicDto
{
  public string Name { get; set;} = string.Empty;
  public string Category { get; set;} = string.Empty;
  public string? Notes { get; set;}
}