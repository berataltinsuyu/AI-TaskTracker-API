using System.Security.Claims;
using AITaskTracker.API.DTOs;
using AITaskTracker.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AITaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(ApiResponse<List<TaskResponseDto>>.ErrorResponse("User id claim not found."));
        }

        var tasks = await _taskService.GetAllAsync(userId.Value);

        return Ok(ApiResponse<List<TaskResponseDto>>.SuccessResponse(
            tasks,
            "Tasks listed successfully."
        ));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(ApiResponse<TaskResponseDto>.ErrorResponse("User id claim not found."));
        }

        var task = await _taskService.GetByIdAsync(id, userId.Value);

        if (task is null)
        {
            return NotFound(ApiResponse<TaskResponseDto>.ErrorResponse("Task not found."));
        }

        return Ok(ApiResponse<TaskResponseDto>.SuccessResponse(
            task,
            "Task retrieved successfully."
        ));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest(ApiResponse<TaskResponseDto>.ErrorResponse("Title is required."));
        }

        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(ApiResponse<TaskResponseDto>.ErrorResponse("User id claim not found."));
        }

        var createdTask = await _taskService.CreateAsync(dto, userId.Value);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdTask.Id },
            ApiResponse<TaskResponseDto>.SuccessResponse(createdTask, "Task created successfully.")
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTaskDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest(ApiResponse<TaskResponseDto>.ErrorResponse("Title is required."));
        }

        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(ApiResponse<TaskResponseDto>.ErrorResponse("User id claim not found."));
        }

        var updatedTask = await _taskService.UpdateAsync(id, userId.Value, dto);

        if (updatedTask is null)
        {
            return NotFound(ApiResponse<TaskResponseDto>.ErrorResponse("Task not found."));
        }

        return Ok(ApiResponse<TaskResponseDto>.SuccessResponse(
            updatedTask,
            "Task updated successfully."
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

        var deleted = await _taskService.DeleteAsync(id, userId.Value);

        if (!deleted)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Task not found."));
        }

        return Ok(ApiResponse<object>.SuccessResponse(null!, "Task deleted successfully."));
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