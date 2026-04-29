using AutoMapper;
using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace QuizSvcSvc.Configurations;
public sealed class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;
    private readonly IHostEnvironment _env;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var method = context.Method;
        var correlationId = context.RequestHeaders.FirstOrDefault(h => h.Key == "x-correlation-id")?.Value ?? "N/A";

        _logger.LogInformation("[gRPC- Interceptor] → {Method} | CorrelationId={CorrelationId}", method, correlationId);

        try
        {
            var response = await continuation(request, context);

            _logger.LogInformation("[gRPC- Interceptor] ← {Method} | CorrelationId={CorrelationId}", method, correlationId
            );

            return response;
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "[gRPC - Interceptor] Validation failed | {Method} | CorrelationId={CorrelationId}", method, correlationId);

            throw new RpcException(new Status(StatusCode.InvalidArgument, $"VALIDATION_ERROR|{ex.Message}"));
        }
        catch (AutoMapperMappingException ex)
        {
            _logger.LogError(ex, "[gRPC - Interceptor] AutoMapper error | {Method} | CorrelationId={CorrelationId}", method, correlationId);

            var detail = _env.IsDevelopment() ? ex.Message : "Mapping configuration error";

            throw new RpcException(
                new Status(StatusCode.Internal, $"MAPPING_ERROR|{detail}")
            );
        }
        catch (RpcException)
        {
            // đã là gRPC → giữ nguyên
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[gRPC - Interceptor] Unhandled error | {Method} | CorrelationId={CorrelationId}", method, correlationId);

            throw new RpcException(new Status(StatusCode.Internal, "INTERNAL_ERROR|Internal server error")
            );
        }
    }
}
