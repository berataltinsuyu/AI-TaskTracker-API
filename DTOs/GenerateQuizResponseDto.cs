namespace AITaskTracker.API.DTOs;

public class GenerateQuizResponseDto
{
    public string Topic { get; set; } = string.Empty;
    public List<QuizQuestionDto> Questions { get; set;} = new();
}