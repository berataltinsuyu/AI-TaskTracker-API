namespace AITaskTracker.API.Entities;

public class DailyLog
{
  public int Id {get ; set;}
  public string Content {get; set;} = string.Empty;
  public DateTime LogDate {get; set;} = DateTime.UtcNow;
  public int UserId {get ; set;}
}