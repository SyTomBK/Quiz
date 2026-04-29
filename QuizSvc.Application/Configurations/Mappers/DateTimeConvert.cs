using Google.Protobuf.WellKnownTypes;

namespace QuizSvc.Application.Configurations.Mappers;

public static class DateTimeConvert
{
    // Proto → Domain
    public static DateTime? ToDateTimeUtc(this Timestamp? ts)
    {
        if (ts == null)
            return null;

        return ts.ToDateTime(); // luôn Utc
    }

    // Domain → Proto
    public static Timestamp? ToTimestampUtc(this DateTime? dt)
    {
        if (!dt.HasValue)
            return null;

        var value = dt.Value;

        if (value.Kind != DateTimeKind.Utc)
        {
            value = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        return Timestamp.FromDateTime(value);
    }
}
