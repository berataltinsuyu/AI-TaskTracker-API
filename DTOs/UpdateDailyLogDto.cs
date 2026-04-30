namespace AITaskTracker.API.DTOs;

public class UpdateDailyLogDto
{
    public string Content { get; set;} = string.Empty;
    public DateTime LogDate { get; set; }
}