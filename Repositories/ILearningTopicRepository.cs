using AITaskTracker.API.Entities;

namespace AITaskTracker.API.Repositories;

public interface ILearningTopicRepository
{
  Task<List<LearningTopic>> GetAllAsync();
  Task<LearningTopic?> GetByIdAsync(int id);
  Task AddAsync(LearningTopic learningTopic);
  void Delete(LearningTopic learningTopic);
  Task SaveChangesAsync();
}
