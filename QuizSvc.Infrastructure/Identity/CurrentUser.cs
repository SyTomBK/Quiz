namespace QuizSvc.Infrastructure.Identity;
public class CurrentUser : ICurrentUser
{
    private readonly GrpcUserContext _context;

    public CurrentUser(GrpcUserContext context)
    {
        _context = context;
    }
    public string UserName => _context.UserName ?? "System";
    public Guid UserId => _context.UserId ?? Guid.Empty;
}
