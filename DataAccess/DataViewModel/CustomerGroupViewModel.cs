using CustomerTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataViewModel
{
    public class CustomerGroupViewModel
    {
        public int CustomerId { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public List<Group?>? Groups { get; set; }
    }
}
