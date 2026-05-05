using AITaskTracker.API.DTOs;

namespace AITaskTracker.API.Services;

public interface IAiService
{
    Task<string> SummarizeAsync(string text);
    Task<GenerateQuizResponseDto> GenerateQuizAsync(string topic);
}