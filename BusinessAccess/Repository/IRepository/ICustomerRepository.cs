using CustomerTask;
using DataAccess.DataViewModel;
using DataAccess.DTOs;
using DataAccess.Repository.IRepository;

namespace BusinessAccess.Repository.IRepository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Addthis(Customer model);
        bool CheckACExist(int id, string acno);
        bool CheckCompanyNameExist(int id, string name);
        void DeleteCustomer(string acno);
        void DeleteCustomerlist(List<int> ids);
        void Editthis(Customer model);
        List<string> GetAllAcoountNumber();
        PageFilterResponseDTO<CustomerListViewModel> GetCustomerList(PageFilterRequestDTO<CustomerSearchFilterDTO> pageFilterDTO);
        Customer? GetInfoOfAC(string acno);
        Customer? GetInfoOfId(int id);

        List<CustomerListViewModel> GetCustomerViewList(List<Customer> customertable);
        List<Group> CustomerGroupDetail(int id, string search);

        List<Group> AllGroups();
        bool AddGroupInCustomer(int customerid, int groupid, string group);
        void DeleteGroupFromCustomer(int customerid, int groupid);
        List<Supplier> GetSupplierOfGroup(int groupid, string search);

        List<Supplier> GetNullSupplier(string search);
        CustomerGroupViewModel GetGroupCountandName(string acno);
        int AddThisContact(Contact contact);
        Contact GetContactDataById(int contactid);
        List<Contact> GetContactListOfCustomer(int customerId, string search);
        void DeletthisContact(int contactid);
    }
}
