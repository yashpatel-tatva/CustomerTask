using CustomerTask;

namespace DataAccess.DataViewModel
{
    public class GroupAndSuppliersViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public List<Supplier> SupplierOfGroup { get; set; } = new List<Supplier> { new Supplier() };
        public List<Supplier> NullSupplier { get; set; } = new List<Supplier> { new Supplier() };
    }
}
