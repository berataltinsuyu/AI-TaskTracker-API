using AITaskTracker.API.DTOs;
using AITaskTracker.API.Entities;
using AITaskTracker.API.Repositories;

namespace AITaskTracker.API.Services;

public class DailyLogService : IDailyLogService
{
    private readonly IDailyLogRepository _dailyLogRepository;

    public DailyLogService(IDailyLogRepository dailyLogRepository)
    {
        _dailyLogRepository = dailyLogRepository;
    }

    public async Task<List<DailyLogResponseDto>> GetAllAsync()
    {
        var logs = await _dailyLogRepository.GetAllAsync();

        return logs.Select(log => new DailyLogResponseDto
        {
            Id = log.Id,
            Content = log.Content,
            LogDate = log.LogDate,
            UserId = log.UserId
        }).ToList();
    }

    public async Task<DailyLogResponseDto?> GetByIdAsync(int id)
    {
        var log = await _dailyLogRepository.GetByIdAsync(id);

        if (log is null)
        {
            return null;
        }

        return new DailyLogResponseDto
        {
            Id = log.Id,
            Content = log.Content,
            LogDate = log.LogDate,
            UserId = log.UserId
        };
    }

    public async Task<DailyLogResponseDto> CreateAsync(CreateDailyLogDto dto, int userId)
    {
        var dailyLog = new DailyLog
        {
            Content = dto.Content,
            LogDate = dto.LogDate,
            UserId = userId
        };

        await _dailyLogRepository.AddAsync(dailyLog);
        await _dailyLogRepository.SaveChangesAsync();

        return new DailyLogResponseDto
        {
            Id = dailyLog.Id,
            Content = dailyLog.Content,
            LogDate = dailyLog.LogDate,
            UserId = dailyLog.UserId
        };
    }

    public async Task<DailyLogResponseDto?> UpdateAsync(int id, UpdateDailyLogDto dto)
    {
        var log = await _dailyLogRepository.GetByIdAsync(id);

        if (log is null)
        {
            return null;
        }

        log.Content = dto.Content;
        log.LogDate = dto.LogDate;

        await _dailyLogRepository.SaveChangesAsync();

        return new DailyLogResponseDto
        {
            Id = log.Id,
            Content = log.Content,
            LogDate = log.LogDate,
            UserId = log.UserId
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var log = await _dailyLogRepository.GetByIdAsync(id);

        if (log is null)
        {
            return false;
        }

        _dailyLogRepository.Delete(log);
        await _dailyLogRepository.SaveChangesAsync();

        return true;
    }
}