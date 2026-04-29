namespace QuizSvc.Share.Utils;
public class PagedList<T> where T : class
{
    public IReadOnlyList<T> Data { get; init; } = new List<T>();
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }

    public PagedList() { }

    public PagedList(IEnumerable<T> data, int totalCount, int page, int pageSize)
    {
        Data = data?.ToList() ?? new List<T>();
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }

    public static PagedList<T> Create(IEnumerable<T> data, int totalCount, int page, int pageSize)
    {
        return new PagedList<T>(data, totalCount, page, pageSize);
    }

    public static PagedList<T> Empty(int page = 1, int pageSize = 10)
    {
        return new PagedList<T>(new List<T>(), 0, page, pageSize);
    }
}
