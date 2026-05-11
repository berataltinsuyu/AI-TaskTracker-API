using AITaskTracker.API.DTOs;

namespace AITaskTracker.API.Services;

public interface ITaskService
{
  Task<List<TaskResponseDto>> GetAllAsync(int userId);
  Task<TaskResponseDto?> GetByIdAsync(int id, int userId);
  Task<TaskResponseDto> CreateAsync(CreateTaskDto dto, int userId);
  Task<TaskResponseDto?> UpdateAsync(int id, int userId, UpdateTaskDto dto);
  Task <bool> DeleteAsync(int id, int userId);
}