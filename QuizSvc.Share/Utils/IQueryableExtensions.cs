using System.Linq.Expressions;

namespace QuizSvc.Share.Utils;
public static class IQueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
    {
        return condition ? query.Where(predicate) : query;
    }
    // Guid?
    public static IQueryable<T> WhereIfNotEmpty<T>(this IQueryable<T> query, Guid? value, Expression<Func<T, bool>> predicate)
    {
        return value.HasValue && value.Value != Guid.Empty
            ? query.Where(predicate) : query;
    }

    // Dành cho String: Chặn null, rỗng, và dấu cách
    public static IQueryable<T> WhereIfNotEmpty<T>(this IQueryable<T> query, string? value, Expression<Func<T, bool>> predicate)
    {
        return !string.IsNullOrWhiteSpace(value) ? query.Where(predicate) : query;
    }

    // Dành cho Số/Enum: Chặn null và giá trị 0 (Unknown)
    public static IQueryable<T> WhereIfHasValue<T, TEnum>(this IQueryable<T> query, TEnum? value, Expression<Func<T, bool>> predicate)
        where TEnum : struct, Enum
    {
        // Sử dụng EqualityComparer để check mặc định (thường là 0) mà không cần ép kiểu Convert
        return value.HasValue && !EqualityComparer<TEnum>.Default.Equals(value.Value, default)
            ? query.Where(predicate) : query;
    }

    // Dành cho List: Chặn null và danh sách rỗng
    public static IQueryable<T> WhereIfAny<T, TProperty>(this IQueryable<T> query, IEnumerable<TProperty>? values, Expression<Func<T, bool>> predicate)
    {
        return values != null && values.Any() ? query.Where(predicate) : query;
    }
   
}