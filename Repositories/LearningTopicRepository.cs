using AITaskTracker.API.Data;
using AITaskTracker.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AITaskTracker.API.Repositories;

public class LearningTopicRepository : ILearningTopicRepository
{
  private readonly AppDbContext _context;

  public LearningTopicRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<List<LearningTopic>> GetAllAsync()
  {
    return await _context.LearningTopics
      .OrderBy(x=> x.Category)
      .ThenBy(x => x.Name)
      .ToListAsync();
  }

  public async Task<LearningTopic?> GetByIdAsync(int id)
  {
    return await _context.LearningTopics.FindAsync(id);
  }

  public async Task AddAsync(LearningTopic learningTopic)
  {
    await _context.LearningTopics.AddAsync(learningTopic);
  }
  public void Delete(LearningTopic learningTopic)
  {
    _context.LearningTopics.Remove(learningTopic);
  }

  public async Task SaveChangesAsync()
  {
    await _context.SaveChangesAsync();
  }
}