using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuizSvc.Application.Commands.Interactions;

namespace QuizSvc.Infrastructure.Workers;

public class LeadScoringJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<LeadScoringJob> _logger;
    private readonly IConfiguration _configuration;

    public LeadScoringJob(IServiceProvider serviceProvider, ILogger<LeadScoringJob> logger, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("LeadScoringJob is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Lấy khoảng thời gian chạy từ cấu hình (mặc định là 60 giây)
                var intervalSeconds = _configuration.GetValue<int>("LeadScoringJob:IntervalSeconds", 60);
                
                using (var scope = _serviceProvider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var batchSize = _configuration.GetValue<int>("LeadScoringJob:BatchSize", 100);

                    _logger.LogInformation("LeadScoringJob is running aggregation at {time}", DateTimeOffset.Now);
                    
                    await mediator.Send(new AggregateLeadScoreCommand { BatchSize = batchSize }, stoppingToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(intervalSeconds), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("LeadScoringJob is stopping.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing LeadScoringJob.");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Chờ 30s rồi thử lại nếu lỗi
            }
        }
    }
}
