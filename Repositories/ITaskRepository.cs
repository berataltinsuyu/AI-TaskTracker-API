using AITaskTracker.API.Entities;

namespace AITaskTracker.API.Repositories;

public interface ITaskRepository
{
  Task<List<TaskItem>> GetAllAsync();
  Task<TaskItem?> GetByIdAsync(int id);
  Task AddAsync(TaskItem taskItem);
  void Delete(TaskItem taskItem);
  Task SaveChangeAsync();
  Task SaveChangesAsync();
}