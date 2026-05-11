using AITaskTracker.API.Data;
using AITaskTracker.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AITaskTracker.API.Repositories;

public class DailyLogRepository : IDailyLogRepository
{
    private readonly AppDbContext _context;

    public DailyLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DailyLog>> GetAllByUserIdAsync(int userId)
    {
        return await _context.DailyLogs
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.LogDate)
            .ToListAsync();
    }

    public async Task<DailyLog?> GetByIdAndUserIdAsync(int id, int userId)
    {
        return await _context.DailyLogs
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
    }

    public async Task AddAsync(DailyLog dailyLog)
    {
        await _context.DailyLogs.AddAsync(dailyLog);
    }

    public void Delete(DailyLog dailyLog)
    {
        _context.DailyLogs.Remove(dailyLog);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}