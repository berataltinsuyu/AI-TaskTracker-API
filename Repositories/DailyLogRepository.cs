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

    public async Task<List<DailyLog>> GetAllAsync()
    {
        return await _context.DailyLogs
            .OrderByDescending(x => x.LogDate)
            .ToListAsync();
    }

    public async Task<DailyLog?> GetByIdAsync(int id)
    {
        return await _context.DailyLogs.FindAsync(id);
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

