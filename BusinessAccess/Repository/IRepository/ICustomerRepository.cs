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
        //int GetCustomerCount(string search);
        PageFilterResponseDTO<CustomerListViewModel> GetCustomerList(PageFilterRequestDTO<CustomerListViewModel> pageFilterDTO);
        Customer GetInfoOfAC(string acno);
        Customer GetInfoOfId(int id);

        List<CustomerListViewModel> GetCustomerViewList(List<Customer> customertable);
    }
}
