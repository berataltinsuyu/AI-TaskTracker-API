using AITaskTracker.API.Data;
using AITaskTracker.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AITaskTracker.API.Repositories;

public class TaskRepository : ITaskRepository
{
  private readonly AppDbContext _context;

  public TaskRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<List<TaskItem>> GetAllAsync()
  {
    return await _context.TaskItems
      .OrderByDescending(x=>x.CreatedAt)
      .ToListAsync();
  }

  public async Task<TaskItem?> GetByIdAsync(int id)
  {
    return await _context.TaskItems.FindAsync(id);
  }

  public async Task AddAsync(TaskItem taskItem)
  {
    await _context.TaskItems.AddAsync(taskItem);
  }

  public void Delete(TaskItem taskItem)
  {
    _context.TaskItems.Remove(taskItem);
  }

  public async Task SaveChangeAsync()
  {
    await _context.SaveChangesAsync();
  }

  public Task SaveChangesAsync()
  {
    throw new NotImplementedException();
  }
}