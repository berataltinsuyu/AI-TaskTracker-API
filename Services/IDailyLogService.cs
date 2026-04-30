using AITaskTracker.API.DTOs;

namespace AITaskTracker.API.Services;

public interface IDailyLogService
{
    Task<List<DailyLogResponseDto>> GetAllAsync();

    Task<DailyLogResponseDto?> GetByIdAsync(int id);

    Task<DailyLogResponseDto> CreateAsync(CreateDailyLogDto dto, int userId);

    Task<DailyLogResponseDto?> UpdateAsync(int id, UpdateDailyLogDto dto);

    Task<bool> DeleteAsync(int id);
}