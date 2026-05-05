namespace AITaskTracker.API.Services;

public interface IAiService
{
    Task<string> SummarizeAsync(string text);
}