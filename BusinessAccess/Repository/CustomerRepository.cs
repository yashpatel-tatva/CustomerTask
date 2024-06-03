using BusinessAccess.Repository.IRepository;
using CustomerTask;
using DataAccess.DataViewModel;
using DataAccess.DTOs;
using DataAccess.Repository;

namespace BusinessAccess.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly IPaginationRepository<CustomerListViewModel, CustomerSearchFilterDTO> _paginationRepository;
        public CustomerRepository(CustomerDbContext db, IPaginationRepository<CustomerListViewModel, CustomerSearchFilterDTO> paginationRepository) : base(db)
        {
            _db = db;
            _paginationRepository = paginationRepository;
        }

        public void Addthis(Customer model)
        {
            model.Isdelete = false;
            _db.Customers.Add(model);
            _db.SaveChanges();
        }

        public bool CheckACExist(int id, string acno)
        {

            return _db.Customers.Any(x => x.Ac == acno && x.Isdelete == false && x.Id != id);
        }

        public bool CheckCompanyNameExist(int id, string name)
        {
            return _db.Customers.Any(x => x.Name == name && x.Isdelete == false && x.Id != id);

        }

        public void DeleteCustomer(string acno)
        {
            Customer cust = _db.Customers.FirstOrDefault(x => x.Ac == acno);
            cust.Isdelete = true;
            _db.Customers.Update(cust);
            _db.SaveChanges();
        }

        public void DeleteCustomerlist(List<int> ids)
        {
            List<Customer?> customerlist = ids.Select(id =>
            {
                Customer? customer = _db.Customers.FirstOrDefault(x => x.Id == id);
                if (customer != null)
                {
                    customer.Isdelete = true;
                }
                return customer;
            }).ToList();
            _db.Customers.UpdateRange(customerlist);
            _db.SaveChanges();
        }

        public void Editthis(Customer model)
        {
            _db.Customers.Update(model);
            _db.SaveChanges();
        }

        public List<string> GetAllAcoountNumber()
        {
            List<string> customer = _db.Customers.Where(x => x.Isdelete == false).Select(x => x.Ac).ToList();
            customer.Sort();
            return customer;
        }

        public PageFilterResponseDTO<CustomerListViewModel> GetCustomerList(PageFilterRequestDTO<CustomerSearchFilterDTO> pageFilter)
        {
            string search = pageFilter.search;
            int currentpage = pageFilter.currentpage;
            int pagesize = pageFilter.pagesize;
            List<CustomerListViewModel> model = GetCustomerViewList(_db.Customers.OrderBy(x => x.Ac).Where(x => x.Isdelete == false).ToList());
            PageFilterResponseDTO<CustomerListViewModel> customerresponse = _paginationRepository.GetPagedData(model, pageFilter);

            return customerresponse;
        }

        public List<CustomerListViewModel> GetCustomerViewList(List<Customer> customertable)
        {
            return customertable.Select(c =>
               {
                   CustomerListViewModel model = new CustomerListViewModel();
                   model.Id = c.Id;
                   model.AC = c.Ac;
                   model.Name = c.Name;
                   model.PostCode = c.Postcode;
                   model.Country = c.Country;
                   model.Telephone = c.Telephone;
                   model.Relationship = c.Relation;
                   model.currency = c.Currency;
                   return model;
               }).ToList<CustomerListViewModel>();
        }

        public Customer GetInfoOfAC(string acno)
        {
            return _db.Customers.FirstOrDefault(x => x.Ac == acno && x.Isdelete == false);
        }

        public Customer GetInfoOfId(int id)
        {
            return _db.Customers.FirstOrDefault(x => x.Id == id && x.Isdelete == false);
        }
    }
}
