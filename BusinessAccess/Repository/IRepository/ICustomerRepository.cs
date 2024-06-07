using CustomerTask;
using DataAccess.DataViewModel;
using DataAccess.DTOs;
using DataAccess.Repository.IRepository;

namespace BusinessAccess.Repository.IRepository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task UpSertCustomer(Customer model);
        Task<bool> CheckACExist(int id, string acNo);
        Task<bool> CheckCompanyNameExist(int id, string name);
        Task DeleteCustomerlist(List<int> ids);
        Task EditCustomer(Customer model);
        Task<List<string>> GetAllAcoountNumber();
        PageFilterResponseDTO<CustomerListViewModel> GetCustomerList(PageFilterRequestDTO<CustomerSearchFilterDTO> pageFilterDTO);
        Task<Customer> GetInfoOfAC(string acNo);
        Task<Customer?> GetInfoOfId(int id);

        List<CustomerListViewModel> GetCustomerViewList(List<Customer> customertable);
        Task<List<GroupViewModel>> CustomerGroupDetail(int id, string Search);

        Task<List<Group>> AllGroups();
        Task<bool> UpSertCustomerGroup(int customerId, int GroupId, string group);
        Task DeleteGroupCustomer(int customerId, int GroupId);
        Task<List<Supplier>> GetSupplierOfGroup(int GroupId, string Search);

        Task<List<Supplier>> GetNullSupplier(string Search);
        Task<CustomerGroupViewModel> GetGroupCountandName(string acNo);
        Task<int> UpSertCustomerContact(Contact Contact);
        Task<ContactViewModel> GetContactDataById(int contactId);
        Task<List<ContactViewModel>> GetContactListCustomer(int customerId, string Search);
        Task DeletthisContact(int contactId);

        CustomerDetailViewModel GetCustomerModel(Customer customer);
        Customer GetTableCustomer(CustomerDetailViewModel customer);
        Task<bool> CustomerGroupExist(int customerId, int GroupId, string Groupname);
    }
}
