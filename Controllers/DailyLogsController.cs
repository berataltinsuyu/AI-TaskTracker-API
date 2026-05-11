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
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(ApiResponse<List<DailyLogResponseDto>>.ErrorResponse("User id claim not found."));
        }

        var logs = await _dailyLogService.GetAllAsync(userId.Value);

        return Ok(ApiResponse<List<DailyLogResponseDto>>.SuccessResponse(
            logs,
            "Daily logs listed successfully."
        ));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(ApiResponse<DailyLogResponseDto>.ErrorResponse("User id claim not found."));
        }

        var log = await _dailyLogService.GetByIdAsync(id, userId.Value);

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

        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(ApiResponse<DailyLogResponseDto>.ErrorResponse("User id claim not found."));
        }

        var createdLog = await _dailyLogService.CreateAsync(dto, userId.Value);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdLog.Id },
            ApiResponse<DailyLogResponseDto>.SuccessResponse(createdLog, "Daily log created successfully.")
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateDailyLogDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Content))
        {
            return BadRequest(ApiResponse<DailyLogResponseDto>.ErrorResponse("Content is required."));
        }

        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(ApiResponse<DailyLogResponseDto>.ErrorResponse("User id claim not found."));
        }

        var updatedLog = await _dailyLogService.UpdateAsync(id, dto, userId.Value);

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
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(ApiResponse<object>.ErrorResponse("User id claim not found."));
        }

        var deleted = await _dailyLogService.DeleteAsync(id, userId.Value);

        if (!deleted)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Daily log not found."));
        }

        return Ok(ApiResponse<object>.SuccessResponse(null!, "Daily log deleted successfully."));
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return null;
        }

        return int.Parse(userIdClaim);
    }
}