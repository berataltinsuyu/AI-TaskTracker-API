using AITaskTracker.API.DTOs;

namespace AITaskTracker.API.Services;

public interface ILearningTopicService
{
    Task<List<LearningTopicResponseDto>> GetAllAsync();

    Task<LearningTopicResponseDto?> GetByIdAsync(int id);

    Task<LearningTopicResponseDto> CreateAsync(CreateLearningTopicDto dto, int userId);

    Task<LearningTopicResponseDto?> UpdateAsync(int id, UpdateLearningTopicDto dto);

    Task<bool> DeleteAsync(int id);
}