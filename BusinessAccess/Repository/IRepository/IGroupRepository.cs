using CustomerTask;
using DataAccess.DataViewModel;
using DataAccess.Repository.IRepository;

namespace BusinessAccess.Repository.IRepository
{
    public interface IGroupRepository : IRepository<Group>
    {
        void EditSupplierinGroup(int groupId, List<int> removeFromGroup, List<int> addToGroup);
        CustomerGroupViewModel GetCustomerGroupModel(int customerId, int groupId);
        Task<bool> SelectGroupInCustomer(int GroupId, bool isSelect);
        Task<bool> UnSelectAllGroupInCustomer(int customerId);
    }
}
