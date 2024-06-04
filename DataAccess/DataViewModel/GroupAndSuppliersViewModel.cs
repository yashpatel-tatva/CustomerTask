using CustomerTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataViewModel
{
    public class GroupAndSuppliersViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<Supplier> SupplierOfGroup { get; set; }
        public List<Supplier> NullSupplier { get; set; }
    }
}
