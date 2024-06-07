namespace DataAccess.DTOs
{
    public class PageFilterRequestDTO<T>
    {
        public string? search;

        public string? Search { get; set; } = null;
        public int currentpage { get; set; } = 1;
        public int pagesize { get; set; } = 10;
        public string OrderByColumnName { get; set; } = string.Empty;
        public bool OrderBy { get; set; }

        public T? SearchByColumns { get; set; }

    }
}
