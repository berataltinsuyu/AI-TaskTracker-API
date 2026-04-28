using AITaskTracker.API.DTOs;

namespace AITaskTracker.API.Services;

public interface ITaskService
{
  Task<List<TaskResponseDto>> GetAllAsync();
  Task<TaskResponseDto?> GetByIdAsync(int id);
  Task<TaskResponseDto> CreateAsync(CreateTaskDto dto);
  Task<TaskResponseDto?> UpdateAsync(int id, UpdateTaskDto dto);
  Task <bool> DeleteAsync(int id);
}