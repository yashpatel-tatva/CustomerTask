using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class PageFilterResponseDTO<T>
    {
        public int TotalColumn { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public string OrderColumnName { get; set; } = string.Empty;
        public bool OrderBy { get; set; }
        public int TotalRecords { get; set; }
        public List<T> Data { get; set; } 
    }
}
