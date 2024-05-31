using BusinessAccess.Repository.IRepository;
using CustomerTask;
using DataAccess.DataViewModel;
using DataAccess.DTOs;
using DataAccess.Repository;

namespace BusinessAccess.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly IPaginationRepository<Customer> _paginationRepository;
        public CustomerRepository(CustomerDbContext db, IPaginationRepository<Customer> paginationRepository) : base(db)
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
            List<Customer?> customer = ids.Select(id =>
            {
                Customer customer = _db.Customers.FirstOrDefault(x => x.Id == id);
                if (customer != null)
                {
                    customer.Isdelete = true;
                }
                return customer;
            }).ToList();
            _db.Customers.UpdateRange(customer);
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

        public int GetCustomerCount(string search)
        {
            List<Customer> customer = _db.Customers.Where(x => x.Isdelete == false).ToList();
            if (search != null)
            {
                customer = customer.Where(x =>
                ((x.Ac != null) && (x.Ac.ToLower().Contains(search.ToLower()))) ||
                ((x.Name != null) && (x.Name.ToLower().StartsWith(search.ToLower()))) ||
                ((x.Postcode != null) && (x.Postcode.ToLower().StartsWith(search.ToLower()))) ||
                ((x.Country != null) && (x.Country.ToLower().StartsWith(search.ToLower()))) ||
                ((x.Telephone != null) && (x.Telephone.ToLower().Contains(search.ToLower()))) ||
                ((x.Relation != null) && (x.Relation.ToLower().StartsWith(search.ToLower()))) ||
                ((x.Currency != null) && (x.Currency.ToLower().StartsWith(search.ToLower())))
                ).ToList();
            }

            return customer.Count();
        }

        public List<CustomerListViewModel> GetCustomerList(PageFilterDTO pageFilter)
        {
            string search = pageFilter.search;
            int currentpage = pageFilter.currentpage;
            int pagesize = pageFilter.pagesize;
            List<Customer> customer = _db.Customers.OrderBy(x => x.Ac).Where(x => x.Isdelete == false).ToList();
            List<Customer> customerslist = new List<Customer>();
            if (search != null)
            {
                customerslist.AddRange(customer.Where(x => (x.Ac != null) && (x.Ac.ToString().ToLower().Contains(search.ToLower()))));
                customerslist.AddRange(customer.Where(x => (x.Name != null) && (x.Name.ToString().ToLower().StartsWith(search.ToLower()))));
                customerslist.AddRange(customer.Where(x => (x.Postcode != null) && (x.Postcode.ToString().ToLower().StartsWith(search.ToLower()))));
                customerslist.AddRange(customer.Where(x => (x.Country != null) && (x.Country.ToString().ToLower().StartsWith(search.ToLower()))));
                customerslist.AddRange(customer.Where(x => (x.Telephone != null) && (x.Telephone.ToString().ToLower().Contains(search.ToLower()))));
                customerslist.AddRange(customer.Where(x => (x.Relation != null) && (x.Relation.ToString().ToLower().StartsWith(search.ToLower()))));
                customerslist.AddRange(customer.Where(x => (x.Currency != null) && (x.Currency.ToString().ToLower().StartsWith(search.ToLower()))));
            }
            else
            {
                customerslist = customer;
            }
            var sortParameters = new Dictionary<Func<Customer, object>, int>    {
                { c => c.Ac, pageFilter.acsort },
                { c => c.Name, pageFilter.namesort },
                { c => c.Postcode, pageFilter.pcsort },
                { c => c.Country, pageFilter.cntrysort },
                { c => c.Telephone, pageFilter.tpsort },
                { c => c.Relation, pageFilter.rlsnsort },
                { c => c.Currency, pageFilter.currsort },
            };
            foreach (var sortParameter in sortParameters)
            {
                if (sortParameter.Value == 1)
                {
                    customerslist = customerslist.AsQueryable().OrderBy(sortParameter.Key).ToList();
                }
                else if (sortParameter.Value == 2)
                {
                    customerslist = customerslist.OrderByDescending(sortParameter.Key).ToList();
                }
            }
            customerslist = _paginationRepository.GetPagedData(customerslist, currentpage, pagesize);
            List<CustomerListViewModel> model = GetCustomerViewList(customerslist);

            return model;
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
