namespace TodoApi.Dtos
{
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public long TotalItems { get; set; }
        public int TotalPages { get; set; }
        public T[] Items { get; set; } = Array.Empty<T>();
    }
}
