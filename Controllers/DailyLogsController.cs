using System.Security.Claims;
using AITaskTracker.API.DTOs;
using AITaskTracker.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AITaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DailyLogsController : ControllerBase
{
  private readonly IDailyLogService _dailyLogService;

  public DailyLogsController(IDailyLogService dailyLogService)
  {
    _dailyLogService = dailyLogService;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var logs = await _dailyLogService.GetAllAsync();

    return Ok(ApiResponse<List<DailyLogResponseDto>>.SuccessResponse
    (
      logs,
      "Daily logs listes succesfully."
    ));
  }
  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(int id)
  {
      var log = await _dailyLogService.GetByIdAsync(id);

      if (log is null)
      {
          return NotFound(ApiResponse<DailyLogResponseDto>.ErrorResponse("Daily log not found."));
      }

      return Ok(ApiResponse<DailyLogResponseDto>.SuccessResponse(
          log,
          "Daily log retrieved successfully."
      ));
  }
  [HttpPost]
  public async Task<IActionResult> Create(CreateDailyLogDto dto)
  {
    if (string.IsNullOrWhiteSpace(dto.Content))
    {
      return BadRequest(ApiResponse<DailyLogResponseDto>.ErrorResponse("Content is required."));
    }

    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrWhiteSpace(userIdClaim))
    {
      return Unauthorized(ApiResponse<DailyLogResponseDto>.ErrorResponse("User id claim not found."));
    }

    var userId = int.Parse(userIdClaim);

    var createdLog = await _dailyLogService.CreateAsync(dto, userId);

    return CreatedAtAction(
      nameof(GetById),
      new {id = createdLog.Id},
      ApiResponse<DailyLogResponseDto>.SuccessResponse(createdLog, "Daily log created successfully.")
    );
  }
  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id,UpdateDailyLogDto dto)
  {
    if (string.IsNullOrWhiteSpace(dto.Content))
    {
      return BadRequest(ApiResponse<DailyLogResponseDto>.ErrorResponse("Content is required."));
    }

    var updatedLog = await _dailyLogService.UpdateAsync(id,dto);

    if (updatedLog is null)
    {
      return NotFound(ApiResponse<DailyLogResponseDto>.ErrorResponse("Daily log not found."));
    }

    return Ok(ApiResponse<DailyLogResponseDto>.SuccessResponse(
      updatedLog,
      "Daily log updated successfully."
    ));
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
      var deleted = await _dailyLogService.DeleteAsync(id);

      if(!deleted)
      {
        return NotFound(ApiResponse<object>.ErrorResponse("Daily log not found."));
      }

      return Ok(ApiResponse<object>.SuccessResponse(null!,"Daily log deleted successfully"));
  }

}