using AITaskTracker.API.Data;
using AITaskTracker.API.DTOs;
using AITaskTracker.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace AITaskTracker.API.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskResponseDto>> GetAllAsync()
    {
        var tasks = await _context.TaskItems
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return tasks.Select(task => new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt
        }).ToList();
    }

    public async Task<TaskResponseDto?> GetByIdAsync(int id)
    {
        var task = await _context.TaskItems.FindAsync(id);

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
            CreatedAt = task.CreatedAt
        };
    }

    public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.TaskItems.AddAsync(task);
        await _context.SaveChangesAsync();

        return new TaskResponseDto
        {
          Id = task.Id,
          Title = task.Title,
          Description = task.Description,
          IsCompleted = task.IsCompleted,
          CreatedAt = task.CreatedAt
        };

    }

    public async Task<TaskResponseDto?> UpdateAsync(int id, UpdateTaskDto dto)
    {
      var task = await _context.TaskItems.FindAsync(id);

      if (task is null)
      {
        return null;
      }

      task.Title = dto.Title;
      task.Description = dto.Description;
      task.IsCompleted = dto.IsCompleted;

      await _context.SaveChangesAsync();

      return new TaskResponseDto
      {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        IsCompleted = task.IsCompleted,
        CreatedAt = task.CreatedAt
      };
    }

    public async Task<bool> DeleteAsync(int id)
    {
      var task = await _context.TaskItems.FindAsync(id);

      if (task is null)
      {
        return false;
      }
      _context.TaskItems.Remove(task);
      await _context.SaveChangesAsync();

      return true;

    }
}