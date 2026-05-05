using AITaskTracker.API.DTOs;
using AITaskTracker.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AITaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AiController : ControllerBase
{
  private readonly IAiService _aiService;

  public AiController (IAiService aiService)
  {
    _aiService = aiService;
  }

  [HttpPost("summarize")]
  public async Task<IActionResult> Summarize(SummarizeRequestDto dto)
  {
    if (string.IsNullOrWhiteSpace(dto.Text))
    {
      return BadRequest(ApiResponse<SummarizeResponseDto>.ErrorResponse("Text is required."));
    }

    var summary = await _aiService.SummarizeAsync(dto.Text);

    var response = new SummarizeResponseDto
    {
      Summary = summary
    };

    return Ok(ApiResponse<SummarizeResponseDto>.SuccessResponse(
      response,
      "Text summarized successfully."
    ));
  }

  [HttpPost("generate-quiz")]
  public async Task<IActionResult> GenerateQuiz(GenerateQuizRequestDto dto)
  {
      if (string.IsNullOrWhiteSpace(dto.Topic))
      {
          return BadRequest(ApiResponse<GenerateQuizResponseDto>.ErrorResponse("Topic is required."));
      }

      var quiz = await _aiService.GenerateQuizAsync(dto.Topic);

      return Ok(ApiResponse<GenerateQuizResponseDto>.SuccessResponse(
          quiz,
          "Quiz generated successfully."
      ));
  }


}