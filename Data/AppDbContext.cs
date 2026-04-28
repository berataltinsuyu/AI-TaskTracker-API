using AITaskTracker.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AITaskTracker.API.Data;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<DailyLog> DailyLogs => Set<DailyLog>();
    public DbSet<LearningTopic> LearningTopics => Set <LearningTopic>();
  
}