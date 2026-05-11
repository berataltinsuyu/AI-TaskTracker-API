using AITaskTracker.API.DTOs;

namespace AITaskTracker.API.Services;

public interface IDailyLogService
{
    Task<List<DailyLogResponseDto>> GetAllAsync(int userId);

    Task<DailyLogResponseDto?> GetByIdAsync(int id, int userId);

    Task<DailyLogResponseDto> CreateAsync(CreateDailyLogDto dto, int userId);

    Task<DailyLogResponseDto?> UpdateAsync(int id, UpdateDailyLogDto dto, int userId);

    Task<bool> DeleteAsync(int id, int userId);
}