namespace QuizSvc.Application.Common.Exceptions;
public static class GrpcExceptions
{
    // 404
    public static RpcException NotFound(string entity, object id)
        => new(new Status(StatusCode.NotFound, $"{entity} with id '{id}' was not found"));

    public static RpcException NotFound(string message)
      => new(new Status(StatusCode.NotFound, message));

    // 400
    public static RpcException InvalidArgument(string message)
      => new(new Status(StatusCode.InvalidArgument, message));

    // 400
    public static RpcException BadRequest(string message)
        => new(new Status(StatusCode.InvalidArgument, message));

    // 401
    public static RpcException Unauthorized(string message = "Unauthorized")
        => new(new Status(StatusCode.Unauthenticated, message));

    // 403
    public static RpcException Forbidden(string message = "Forbidden")
        => new(new Status(StatusCode.PermissionDenied, message));

    // 409 (conflict chung)
    public static RpcException Conflict(string message)
        => new(new Status(StatusCode.Aborted, message));

    // 409 (already exists – semantic rõ ràng hơn)
    public static RpcException AlreadyExists(string entity, object id)
        => new(new Status(StatusCode.AlreadyExists, $"{entity} with id '{id}' already exists"));

    public static RpcException AlreadyExists(string message)
    => new(new Status(StatusCode.AlreadyExists, message));

    // 429
    public static RpcException ResourceExhausted(string message = "Too many requests")
        => new(new Status(StatusCode.ResourceExhausted, message));

    // 412
    public static RpcException FailedPrecondition(string message)
        => new(new Status(StatusCode.FailedPrecondition, message));

    // 400
    public static RpcException OutOfRange(string message)
        => new(new Status(StatusCode.OutOfRange, message));

    // 501
    public static RpcException Unimplemented(string message = "Not implemented")
        => new(new Status(StatusCode.Unimplemented, message));

    // 500
    public static RpcException Internal(string message = "Internal server error")
        => new(new Status(StatusCode.Internal, message));

    // 503
    public static RpcException Unavailable(string message = "Service unavailable")
        => new(new Status(StatusCode.Unavailable, message));

    // 500
    public static RpcException DataLoss(string message = "Data loss")
        => new(new Status(StatusCode.DataLoss, message));

    // Lỗi không xác định (Fallback)
    public static RpcException Unknown(string message = "Unknown error")
        => new(new Status(StatusCode.Unknown, message));

    // Timeout (Client chờ quá lâu hoặc Server xử lý quá hạn)
    public static RpcException DeadlineExceeded(string message = "Deadline exceeded")
        => new(new Status(StatusCode.DeadlineExceeded, message));
}
