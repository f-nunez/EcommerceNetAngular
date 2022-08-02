namespace Fnunez.Ena.API.Helpers;

public class PaginationHelper<T> where T : class
{
    public int Count { get; set; }
    public IReadOnlyList<T> Data { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }

    public PaginationHelper(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Data = data;
    }
}