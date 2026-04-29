using System.Security.Cryptography;

namespace QuizSvc.Infrastructure.Persistence.Commons;
public class CodeGenerator : ICodeGenerator
{
    // Loại bỏ các ký tự dễ gây nhầm lẫn: 0, 1, O, I
    private const string FriendlyChars = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";
    public string Generate(string prefix, int length = 8)
    {
        if (string.IsNullOrEmpty(prefix))
        {
            prefix = string.Empty;
        }
        Span<char> result = stackalloc char[length];
        for (int i = 0; i < length; i++)
        {
            // Lấy số ngẫu nhiên bảo mật từ 0 đến độ dài của FriendlyChars
            int index = RandomNumberGenerator.GetInt32(FriendlyChars.Length);
            result[i] = FriendlyChars[index];
        }

        return $"{prefix.ToUpperInvariant()}-{new string(result)}";
    }
}
