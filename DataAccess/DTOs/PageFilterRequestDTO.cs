namespace DataAccess.DTOs
{
    public class PageFilterRequestDTO<T>
    {
        public string search { get; set; } =null;
        public int currentpage { get; set; } = 1;
        public int pagesize { get; set; } = 10;
        public string OrderByColumnName { get; set; } = string.Empty;
        public bool OrderBy { get; set; }

        public T? SearchByColumns { get; set; }
        //public int acsort { get; set; } = 0;
        //public int namesort { get; set; } = 0;
        //public int pcsort { get; set; } = 0;
        //public int cntrysort { get; set; } = 0;
        //public int tpsort { get; set; } = 0;
        //public int rlsnsort { get; set; } = 0;
        //public int currsort { get; set; } = 0;

    }
}
