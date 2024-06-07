using BusinessAccess.Repository.IRepository;
using CustomerTask;
using DataAccess.DataViewModel;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace BusinessAccess.Repository
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(CustomerDbContext db) : base(db)
        {
        }

        public void EditSupplierinGroup(int GroupId, List<int> RemoveFromGroup, List<int> AddToGroup)
        {
            //List<Supplier> removesuppliers = RemoveFromGroup.Select(x =>
            //{
            //    Supplier s = _db.Suppliers.FirstOrDefault(s => s.Id == x);
            //    s.GroupId = null;
            //    return s;
            //}).ToList();
            //List<Supplier> addsupplier = AddToGroup.Select(x =>
            //{
            //    Supplier s = _db.Suppliers.FirstOrDefault(s => s.Id == x);
            //    s.GroupId = GroupId;
            //    return s;
            //}).ToList();
            //List<Supplier> update = removesuppliers.Concat(addsupplier).ToList();
            //_db.Suppliers.UpdateRange(update);
            //_db.SaveChanges();
        }


        public CustomerGroupViewModel GetCustomerGroupModel(int customerId, int groupId)
        {
            Group group = GetFirstOrDefault(x=>x.Id==groupId);
            return new()
            {
                CustomerId = customerId,
                GroupId = groupId,
                GroupName = group != null ? group.Name : "",
                Groups = new()
            };
        }

        public async Task<bool> SelectGroupInCustomer(int GroupId, bool isselect)
        {
            Group Group = await _db.Groups.FirstOrDefaultAsync(g => g.Id == GroupId) ?? new Group();
            if (Group.Id != 0)
            {
                Group.Isselect = isselect;
                await _db.SaveChangesAsync();
                return true;
            }
            else { return false; }
        }

        public async Task<bool> UnSelectAllGroupInCustomer(int customerId)
        {
            List<Group> g = await _db.Groups.Where(x => x.CustomerId == customerId && !x.Isdelete).ToListAsync();
            g.ForEach(x =>
            {
                x.Isselect = false;
            });
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
