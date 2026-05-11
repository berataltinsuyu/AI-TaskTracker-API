using AITaskTracker.API.DTOs;
using AITaskTracker.API.Entities;
using AITaskTracker.API.Repositories;

namespace AITaskTracker.API.Services;

public class LearningTopicService : ILearningTopicService
{
    private readonly ILearningTopicRepository _learningTopicRepository;

    public LearningTopicService(ILearningTopicRepository learningTopicRepository)
    {
        _learningTopicRepository = learningTopicRepository;
    }

    public async Task<List<LearningTopicResponseDto>> GetAllAsync()
    {
        var topics = await _learningTopicRepository.GetAllAsync();

        return topics.Select(topic => new LearningTopicResponseDto
        {
            Id = topic.Id,
            Name = topic.Name,
            Category = topic.Category,
            Notes = topic.Notes,
            UserId = topic.UserId
        }).ToList();
    }

    public async Task<LearningTopicResponseDto?> GetByIdAsync(int id)
    {
        var topic = await _learningTopicRepository.GetByIdAsync(id);

        if (topic is null)
        {
            return null;
        }

        return new LearningTopicResponseDto
        {
            Id = topic.Id,
            Name = topic.Name,
            Category = topic.Category,
            Notes = topic.Notes,
            UserId = topic.UserId
        };
    }

    public async Task<LearningTopicResponseDto> CreateAsync(CreateLearningTopicDto dto, int userId)
    {
        var learningTopic = new LearningTopic
        {
            Name = dto.Name,
            Category = dto.Category,
            Notes = dto.Notes,
            UserId = userId
        };

        await _learningTopicRepository.AddAsync(learningTopic);
        await _learningTopicRepository.SaveChangesAsync();

        return new LearningTopicResponseDto
        {
            Id = learningTopic.Id,
            Name = learningTopic.Name,
            Category = learningTopic.Category,
            Notes = learningTopic.Notes,
            UserId = learningTopic.UserId
        };
    }

    public async Task<LearningTopicResponseDto?> UpdateAsync(int id, UpdateLearningTopicDto dto)
    {
        var topic = await _learningTopicRepository.GetByIdAsync(id);

        if (topic is null)
        {
            return null;
        }

        topic.Name = dto.Name;
        topic.Category = dto.Category;
        topic.Notes = dto.Notes;

        await _learningTopicRepository.SaveChangesAsync();

        return new LearningTopicResponseDto
        {
            Id = topic.Id,
            Name = topic.Name,
            Category = topic.Category,
            Notes = topic.Notes,
            UserId = topic.UserId
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var topic = await _learningTopicRepository.GetByIdAsync(id);

        if (topic is null)
        {
            return false;
        }

        _learningTopicRepository.Delete(topic);
        await _learningTopicRepository.SaveChangesAsync();

        return true;
    }
}