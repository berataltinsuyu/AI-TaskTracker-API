namespace AITaskTracker.API.DTOs;

public class DailyLogResponseDto
{
    public int Id { get; set; }
    public string Content { get; set;} = string.Empty;
    public DateTime LogDate { get; set; }
    public int UserId { get ; set;} = 0;
}