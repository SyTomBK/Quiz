using QuizSvc.Services;

namespace QuizSvcSvc.Extensions;
public static class GrpcEndpointExtensions
{
    public static WebApplication MapGrpcEndpoints(this WebApplication app)
    {
        app.MapGrpcService<QuizService>();
        return app;
    }
}
    