using AITaskTracker.API.DTOs;
using AITaskTracker.API.Entities;
using AITaskTracker.API.Repositories;

namespace AITaskTracker.API.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskResponseDto>> GetAllAsync(int userId)
    {
        var tasks = await _taskRepository.GetAllByUserIdAsync(userId);

        return tasks.Select(task => new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            UserId = task.UserId
        }).ToList();
    }

    public async Task<TaskResponseDto?> GetByIdAsync(int id, int userId)
    {
        var task = await _taskRepository.GetByIdAndUserIdAsync(id, userId);

        if (task is null)
        {
            return null;
        }

        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            UserId = task.UserId
        };
    }

    public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto, int userId)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();

        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            UserId = task.UserId
        };
    }

    public async Task<TaskResponseDto?> UpdateAsync(int id, int userId, UpdateTaskDto dto)
    {
        var task = await _taskRepository.GetByIdAndUserIdAsync(id, userId);

        if (task is null)
        {
            return null;
        }

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;

        await _taskRepository.SaveChangesAsync();

        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            UserId = task.UserId
        };
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var task = await _taskRepository.GetByIdAndUserIdAsync(id, userId);

        if (task is null)
        {
            return false;
        }

        _taskRepository.Delete(task);
        await _taskRepository.SaveChangesAsync();

        return true;
    }
}