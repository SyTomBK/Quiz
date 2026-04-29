using Google.Protobuf.WellKnownTypes;
using System.Globalization;

namespace QuizSvc.Application.Configurations.Mappers;
public static class SafeConvert
{
     #region gRPC Timestamp (Extension Methods)

    /// Chuyển đổi DateTime? sang Google Protobuf Timestamp.
    /// Đảm bảo dữ liệu luôn ở chuẩn UTC để tránh lỗi gRPC runtime.
    public static Timestamp? ToTimestampUtc(this DateTime? dt)
    {
        if (!dt.HasValue) return null;

        DateTime utcValue = dt.Value.Kind switch
        {
            DateTimeKind.Utc => dt.Value,
            DateTimeKind.Local => dt.Value.ToUniversalTime(),
            // Nếu là Unspecified (thường từ DB), ta giả định là UTC để FromDateTime không lỗi
            _ => DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc)
        };

        return Timestamp.FromDateTime(utcValue);
    }

    /// Chuyển đổi Proto Timestamp sang DateTime? (luôn trả về UTC).
    public static DateTime? ToDateTimeUtc(this Timestamp? ts)
    {
        return ts?.ToDateTime();
    }

    #endregion

    #region Guid & Basic Types

    public static Guid? ToGuidNullable(string? v)
        => Guid.TryParse(v, out var g) ? g : null;

    public static Guid ToGuidRequired(string? v)
    {
        if (!Guid.TryParse(v, out var g))
            throw new AutoMapperMappingException($"Invalid Guid value: {v}");

        return g;
    }

    // Dùng ĐẶC BIỆT cho gRPC (Không bao giờ null)
    public static string ToGuidString(Guid? v)
        => v?.ToString() ?? string.Empty;

    // Hàm tiện ích: Đi thẳng từ string? (DTO) sang string (gRPC)
    public static string ToGrpcGuidString(string? v)
        => Guid.TryParse(v, out var g) ? g.ToString() : string.Empty;

    public static DateTime? ToDate(string? v)
        => DateTime.TryParse(v, out var d) ? d : null;

    public static string? ToDateString(DateTime? v)
        => v?.ToString("O");

    public static decimal? ToDecimal(string? v)
    {
        if (string.IsNullOrWhiteSpace(v)) return null;

        return decimal.TryParse(v, NumberStyles.Any, CultureInfo.InvariantCulture, out var d)
            ? d : null;
    }

    public static string? ToDecimalString(decimal? v)
        => v?.ToString(CultureInfo.InvariantCulture);

    public static bool? ToBool(string? v)
        => bool.TryParse(v, out var b) ? b : null;

    public static string? ToBoolString(bool? v)
        => v?.ToString().ToLower();

    public static int? ToInt(string? v)
        => int.TryParse(v, out var i) ? i : null;

    public static string? ToIntString(int? v)
        => v?.ToString();

    #endregion

    #region Enums (Fixed Ambiguous Reference)

    /// Ép kiểu string sang Enum. Sử dụng System.Enum để tránh xung đột với Google.Protobuf.Enum.
    public static TEnum? ToEnum<TEnum>(string? v)
        where TEnum : struct, System.Enum
    {
        if (System.Enum.TryParse<TEnum>(v, true, out var e))
            return e;

        return null;
    }

    public static string? ToEnumString<TEnum>(TEnum? v)
        where TEnum : struct, System.Enum
        => v?.ToString();

    #endregion

    #region DateTimeOffset

    public static DateTimeOffset? ToDateTimeOffset(string? v)
        => DateTimeOffset.TryParse(v, out var d) ? d : null;

    public static string? ToDateTimeOffsetString(DateTimeOffset? v)
        => v?.ToString("O");

    #endregion

    /// Hàm bổ trợ chuyển đổi object sang string an toàn.
    public static string ToStringSafe(object? v)
    {
        if (v == null) return string.Empty;
        if (v is DateTime dt) return dt.ToString("yyyy-MM-ddTHH:mm:ssZ");
        return v.ToString() ?? string.Empty;
    }
}
