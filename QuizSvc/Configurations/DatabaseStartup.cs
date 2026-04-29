using Microsoft.EntityFrameworkCore;
namespace QuizSvcSvc.Configurations;

public static class DatabaseStartup
{
    public static IApplicationBuilder UseApplicationDatabase<T>(this IApplicationBuilder app, bool isMig) where T : DbContext
    {
        if (isMig)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<T>();
            context.Database.OpenConnection();
            context.Database.Migrate();
            context.Database.EnsureCreated();
        }
        return app;
    }
}
