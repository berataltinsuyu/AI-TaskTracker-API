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

    public async Task<List<LearningTopic>> GetAllByUserIdAsync(int userId)
    {
        return await _context.LearningTopics
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<LearningTopic?> GetByIdAndUserIdAsync(int id, int userId)
    {
        return await _context.LearningTopics
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
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