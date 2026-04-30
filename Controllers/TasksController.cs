using AITaskTracker.API.DTOs;
using AITaskTracker.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AITaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        var tasks = await _taskService.GetAllAsync();

        return Ok(ApiResponse<List<TaskResponseDto>>.SuccessResponse(tasks,
            "Tasks listed successfully."));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskService.GetByIdAsync(id);

        if (task is null)
        {
            return NotFound(ApiResponse<TaskResponseDto>.ErrorResponse("Task not found."));
        }

        return Ok(ApiResponse<TaskResponseDto>.SuccessResponse(
            task, "Task retrieved successfully."));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest(ApiResponse<TaskResponseDto>.ErrorResponse("Title is required."));
        }
        var createdTask = await _taskService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById), 
            new { id = createdTask.Id },
            ApiResponse<TaskResponseDto>.SuccessResponse(createdTask, "Task created succesfully."));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTaskDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest(ApiResponse<TaskResponseDto>.ErrorResponse("Title is required."));
        }
        
        var updatedTask = await _taskService.UpdateAsync(id, dto);

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
        var deleted = await _taskService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Task not found."));
        }

        return Ok(ApiResponse<object>.SuccessResponse(null!,"Task deleted successfully"));
    }
}