using CustomerTask;
using DataAccess.DataViewModel;
using DataAccess.Repository.IRepository;

namespace BusinessAccess.Repository.IRepository
{
    public interface IGroupRepository : IRepository<Group>
    {
        void EditSupplierinGroup(int GroupId, List<int> RemoveFromGroup, List<int> AddToGroup);
        CustomerGroupViewModel GetCustomerGroupModel(int customerId, int groupId);
        Task<bool> SelectGroupInCustomer(int GroupId, bool isselect);
        Task<bool> UnSelectAllGroupInCustomer(int customerId);
    }
}
