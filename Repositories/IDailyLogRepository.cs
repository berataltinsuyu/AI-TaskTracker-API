using AITaskTracker.API.Entities;

namespace AITaskTracker.API.Repositories;

public interface IDailyLogRepository
{
    Task<List<DailyLog>> GetAllByUserIdAsync(int userId);

    Task<DailyLog?> GetByIdAndUserIdAsync(int id, int userId);

    Task AddAsync(DailyLog dailyLog);

    void Delete(DailyLog dailyLog);

    Task SaveChangesAsync();
}