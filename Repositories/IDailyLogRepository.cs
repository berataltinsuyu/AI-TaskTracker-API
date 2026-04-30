using AITaskTracker.API.Entities;

namespace AITaskTracker.API.Repositories;

public interface IDailyLogRepository
{
    Task<List<DailyLog>> GetAllAsync();

    Task<DailyLog?> GetByIdAsync(int id);

    Task AddAsync(DailyLog dailyLog);

    void Delete(DailyLog dailyLog);

    Task SaveChangesAsync();
}