namespace AITaskTracker.API.Entities;
public class LearningTopic
{
  public int Id {get; set;}
  public string Name {get; set;} = string.Empty;
  public string Category {get; set;} = string.Empty;
  public string? Notes {get; set;}
  public int UserId {get; set;}
}