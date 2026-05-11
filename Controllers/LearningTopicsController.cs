using System.Security.Claims;
using AITaskTracker.API.DTOs;
using AITaskTracker.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AITaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LearningTopicsController : ControllerBase
{
    private readonly ILearningTopicService _learningTopicService;

    public LearningTopicsController(ILearningTopicService learningTopicService)
    {
        _learningTopicService = learningTopicService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var topics = await _learningTopicService.GetAllAsync();

        return Ok(ApiResponse<List<LearningTopicResponseDto>>.SuccessResponse(
            topics,
            "Learning topics listed successfully."
        ));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var topic = await _learningTopicService.GetByIdAsync(id);

        if (topic is null)
        {
            return NotFound(ApiResponse<LearningTopicResponseDto>.ErrorResponse("Learning topic not found."));
        }

        return Ok(ApiResponse<LearningTopicResponseDto>.SuccessResponse(
            topic,
            "Learning topic retrieved successfully."
        ));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLearningTopicDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest(ApiResponse<LearningTopicResponseDto>.ErrorResponse("Name is required."));
        }

        if (string.IsNullOrWhiteSpace(dto.Category))
        {
            return BadRequest(ApiResponse<LearningTopicResponseDto>.ErrorResponse("Category is required."));
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized(ApiResponse<LearningTopicResponseDto>.ErrorResponse("User id claim not found."));
        }

        var userId = int.Parse(userIdClaim);

        var createdTopic = await _learningTopicService.CreateAsync(dto, userId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdTopic.Id },
            ApiResponse<LearningTopicResponseDto>.SuccessResponse(
                createdTopic,
                "Learning topic created successfully."
            )
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateLearningTopicDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest(ApiResponse<LearningTopicResponseDto>.ErrorResponse("Name is required."));
        }

        if (string.IsNullOrWhiteSpace(dto.Category))
        {
            return BadRequest(ApiResponse<LearningTopicResponseDto>.ErrorResponse("Category is required."));
        }

        var updatedTopic = await _learningTopicService.UpdateAsync(id, dto);

        if (updatedTopic is null)
        {
            return NotFound(ApiResponse<LearningTopicResponseDto>.ErrorResponse("Learning topic not found."));
        }

        return Ok(ApiResponse<LearningTopicResponseDto>.SuccessResponse(
            updatedTopic,
            "Learning topic updated successfully."
        ));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _learningTopicService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Learning topic not found."));
        }

        return Ok(ApiResponse<object>.SuccessResponse(null!, "Learning topic deleted successfully."));
    }
}