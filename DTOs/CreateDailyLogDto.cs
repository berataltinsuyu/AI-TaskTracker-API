namespace AITaskTracker.API.DTOs;

public class CreateDailyLogDto
{
  public string Content { get; set;} = string.Empty;
  public DateTime LogDate { get; set; } = DateTime.UtcNow;
}