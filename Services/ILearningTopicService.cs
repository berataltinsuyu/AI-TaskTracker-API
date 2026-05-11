using AITaskTracker.API.DTOs;

namespace AITaskTracker.API.Services;

public interface ILearningTopicService
{
    Task<List<LearningTopicResponseDto>> GetAllAsync(int userId);

    Task<LearningTopicResponseDto?> GetByIdAsync(int id, int userId);

    Task<LearningTopicResponseDto> CreateAsync(CreateLearningTopicDto dto, int userId);

    Task<LearningTopicResponseDto?> UpdateAsync(int id, UpdateLearningTopicDto dto, int userId);

    Task<bool> DeleteAsync(int id, int userId);
}