using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace QuizSvc.Infrastructure.Identity;
public sealed class UserContextInterceptor: Interceptor
{
    public override async Task<TResponse> UnaryServerHandler <TRequest, TResponse>(
        TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var httpContext = context.GetHttpContext();
        var userContext = httpContext.RequestServices.GetRequiredService<GrpcUserContext>();

        var headers = context.RequestHeaders;
        userContext.UserName = headers.FirstOrDefault(h => h.Key == "x-user-name")?.Value ?? string.Empty;

        var userIdHeader = headers.FirstOrDefault(h => h.Key == "x-user-id")?.Value;

        userContext.UserId = Guid.Empty;

        if (Guid.TryParse(userIdHeader, out var userId))
        {
            userContext.UserId = userId;
        }

        return await continuation(request, context);

        //var roles =
        //   headers.FirstOrDefault(h => h.Key == "x-user-role")?.Value ?? string.Empty;

        //if (!string.IsNullOrWhiteSpace(roles))
        //{
        //    userContext.Roles = roles
        //        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        //        .ToList();
        //}
    }
}
