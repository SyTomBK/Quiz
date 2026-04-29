using Google.Protobuf.WellKnownTypes;

namespace QuizSvc.Application.Configurations.Mappers;
public class BaseProfile : Profile
{
    public BaseProfile()
    {
        // 1. GLOBAL CONVERTERS (Quy tắc chuyển đổi kiểu dữ liệu)
        // DateTime <-> Timestamp (gRPC)
        CreateMap<DateTime?, Timestamp?>().ConvertUsing(src => SafeConvert.ToTimestampUtc(src));
        CreateMap<DateTime, Timestamp>().ConvertUsing(src =>
            Timestamp.FromDateTime(src.Kind == DateTimeKind.Utc ? src : src.ToUniversalTime()));
        CreateMap<Timestamp, DateTime>().ConvertUsing(src => src.ToDateTime());
        CreateMap<Timestamp?, DateTime?>().ConvertUsing(src => SafeConvert.ToDateTimeUtc(src));

        // Guid <-> String (An toàn cho gRPC - Không bao giờ trả về null)
        CreateMap<Guid?, string>().ConvertUsing(src => src.HasValue ? src.Value.ToString() : string.Empty);
        CreateMap<Guid, string>().ConvertUsing(src => src.ToString());
        CreateMap<string, Guid?>().ConvertUsing(src => SafeConvert.ToGuidNullable(src));
        CreateMap<string?, string>().ConvertUsing(src => SafeConvert.ToStringSafe(src));
        CreateMap<string, string>().ConvertUsing(src => SafeConvert.ToStringSafe(src));
    }
}
