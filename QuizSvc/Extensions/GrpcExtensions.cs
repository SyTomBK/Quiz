using ProtoBuf.Grpc.Server;
using QuizSvc.Infrastructure.Identity;
using QuizSvcSvc.Configurations;

namespace QuizSvcSvc.Extensions;
public static class GrpcExtensions
{
    public static IServiceCollection AddSvcGrpc(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<LoggingInterceptor>();
            options.Interceptors.Add<UserContextInterceptor>();
        });
        services.AddCodeFirstGrpc();
        services.AddGrpcReflection();
        services.AddCodeFirstGrpcReflection();

        return services;
    }
}
