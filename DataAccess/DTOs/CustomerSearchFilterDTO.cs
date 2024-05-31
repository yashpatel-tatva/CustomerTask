using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CustomerSearchFilterDTO
    {
        public string AC { get; set; }
        public string Name { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }
        public string Relationship { get; set; }
        public string currency { get; set; }
    }
}
