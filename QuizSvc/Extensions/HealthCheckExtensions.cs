using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

namespace QuizSvcSvc.Extensions;
public static class HealthCheckExtensions
{
    public static WebApplication MapHealthChecksWithResponse(this WebApplication app)
    {
        app.UseRouting();

        app.MapHealthChecks("/liveness", new HealthCheckOptions
        {
            Predicate = _ => false,
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { status = "alive" }));
            }
        });

        app.MapHealthChecks("/readiness", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var response = new
                {
                    status = report.Status.ToString(),
                    totalDuration = report.TotalDuration.TotalMilliseconds + "ms",
                    results = report.Entries.Select(entry => new
                    {
                        component = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description,
                        duration = entry.Value.Duration.TotalMilliseconds + "ms",
                        tags = entry.Value.Tags
                    })
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
            }
        });

        return app;
    }
}
