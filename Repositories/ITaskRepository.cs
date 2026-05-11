using AITaskTracker.API.Entities;

namespace AITaskTracker.API.Repositories;

public interface ITaskRepository
{
  Task<List<TaskItem>> GetAllByUserIdAsync(int userId);
  Task<TaskItem?> GetByIdAndUserIdAsync(int id, int userId);
  Task AddAsync(TaskItem taskItem);
  void Delete(TaskItem taskItem);
  Task SaveChangesAsync();
}