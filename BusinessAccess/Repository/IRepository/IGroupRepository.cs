using CustomerTask;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccess.Repository.IRepository
{
    public interface IGroupRepository : IRepository<Group>
    {
        void EditSupplierinGroup(int groupid, List<int> removefromgroup, List<int> addtogroup);
        bool SelectGroupInCustomer(int groupid, bool isselect);
        bool UnSelectAllGroupInCustomer(int customerid);
    }
}
