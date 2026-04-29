using Serilog;

namespace QuizSvcSvc.Extensions;
public static class LoggingExtensions
{
    public static IServiceCollection AddSvcLogging(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(services.BuildServiceProvider().GetRequiredService<IConfiguration>())
            .Enrich.FromLogContext()
            .CreateLogger();

        services.AddLogging(lb => lb.AddSerilog(dispose: true));

        return services;
    }
}
