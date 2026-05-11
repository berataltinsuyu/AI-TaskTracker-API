using AITaskTracker.API.Entities;

namespace AITaskTracker.API.Repositories;

public interface ILearningTopicRepository
{
    Task<List<LearningTopic>> GetAllByUserIdAsync(int userId);

    Task<LearningTopic?> GetByIdAndUserIdAsync(int id, int userId);

    Task AddAsync(LearningTopic learningTopic);

    void Delete(LearningTopic learningTopic);

    Task SaveChangesAsync();
}