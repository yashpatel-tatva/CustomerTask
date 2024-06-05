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
        public string Ac { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public int TotalSelectedGroup { get; set; }
        public List<Group> Groups { get; set; } = new List<Group>();

        public List<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
