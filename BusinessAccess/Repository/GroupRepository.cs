using BusinessAccess.Repository.IRepository;
using CustomerTask;
using DataAccess.Repository;

namespace BusinessAccess.Repository
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(CustomerDbContext db) : base(db)
        {
        }

        public void EditSupplierinGroup(int groupid, List<int> removefromgroup, List<int> addtogroup)
        {
            //List<Supplier> removesuppliers = removefromgroup.Select(x =>
            //{
            //    Supplier s = _db.Suppliers.FirstOrDefault(s => s.Id == x);
            //    s.GroupId = null;
            //    return s;
            //}).ToList();
            //List<Supplier> addsupplier = addtogroup.Select(x =>
            //{
            //    Supplier s = _db.Suppliers.FirstOrDefault(s => s.Id == x);
            //    s.GroupId = groupid;
            //    return s;
            //}).ToList();
            //List<Supplier> update = removesuppliers.Concat(addsupplier).ToList();
            //_db.Suppliers.UpdateRange(update);
            //_db.SaveChanges();
        }

        public bool SelectGroupInCustomer(int groupid, bool isselect)
        {
            Group group = _db.Groups.FirstOrDefault(g => g.Id == groupid) ?? new Group();
            if (group.Id != 0)
            {
                group.Isselect = isselect;
                _db.SaveChanges();
                return true;
            }
            else { return false; }
        }

        public bool UnSelectAllGroupInCustomer(int customerid)
        {
            List<Group> g = _db.Groups.Where(x=>x.CustomerId == customerid && !x.Isdelete).ToList();
            g.ForEach(x =>
            {
                x.Isselect = false;
            });
            _db.SaveChanges();
            return true;
        }
    }
}
