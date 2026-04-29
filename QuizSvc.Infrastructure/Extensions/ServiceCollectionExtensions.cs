using Microsoft.Extensions.DependencyInjection;
using QuizSvc.Application.Services.Questions;
using QuizSvc.Application.Services.Quizes;
using QuizSvc.Infrastructure.Identity;
using QuizSvc.Infrastructure.Persistence.Commons;
using QuizSvc.Infrastructure.Persistence.Repositories;
using QuizSvc.Infrastructure.Workers;

namespace QuizSvc.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // Register Repository
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<GrpcUserContext>();
        services.AddSingleton<ICodeGenerator, CodeGenerator>();

        services.AddScoped<ICampaignRepository, CampaignRepository>();
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IDimensionRepository, DimensionRepository>();
        services.AddScoped<IQuizAttemptRepository, QuizAttemptRepository>();
        services.AddScoped<IQuizResultRepository, QuizResultRepository>();
        services.AddScoped<ILeadRepository, LeadRepository>();
        services.AddScoped<IInteractionRepository, InteractionRepository>();
        services.AddScoped<IRecommendationRepository, RecommendationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddHostedService<LeadScoringJob>();
        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<ICampaignQuizSettingRepository, CampaignQuizSettingRepository>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<IAnswerDimensionScoresRepository, AnswerDimensionScoresRepository>();
        services.AddScoped<IScoringRuleRepository, ScoringRuleRepository>();

        return services;
    }
}
