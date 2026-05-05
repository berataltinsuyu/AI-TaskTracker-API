namespace AITaskTracker.API.DTOs;

public class QuizQuestionDto
{
    public string Question {get ; set;} = string.Empty;
    public List<string> Options { get ; set;} = new();
    public string Answer { get; set;} = string.Empty;

}