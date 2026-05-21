using AITaskTracker.API.Repositories;
using AITaskTracker.API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AITaskTracker.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ITaskRepository, TaskRepository>();

        services.AddScoped<IDailyLogRepository, DailyLogRepository>();
        services.AddScoped<IDailyLogService, DailyLogService>();

        services.AddScoped<ILearningTopicRepository, LearningTopicRepository>();
        services.AddScoped<ILearningTopicService, LearningTopicService>();

        services.AddScoped<IDashboardService, DashboardService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddHttpClient<IAiService, AiService>();

        return services;
    }
}
