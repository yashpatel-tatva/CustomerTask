using BusinessAccess.Repository.IRepository;
using CustomerTask;
using DataAccess.DataViewModel;
using DataAccess.DTOs;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

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
            if (string.IsNullOrEmpty(name)) return false;
            return _db.Customers.Any(x => x.Name.Trim().ToLower() == name.Trim().ToLower() && x.Isdelete == false && x.Id != id);
        }

        public void DeleteCustomer(string acno)
        {
            Customer? cust = _db.Customers.FirstOrDefault(x => x.Ac == acno);
            cust.Isdelete = true;

            //_db.Customers.Update(cust);

            _db.SaveChanges();
        }

        public void DeleteCustomerlist(List<int> ids)
        {
            var custs = _db.Customers.Where(x => ids.Contains(x.Id) && (x.Isdelete == false)).ToList();
            custs.ForEach(x =>
            {
                x.Isdelete = true;
            });

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
            PageFilterResponseDTO<CustomerListViewModel> customerresponse = _paginationRepository
                .GetPagedData(GetCustomerViewList(_db.Customers.OrderBy(x => x.Ac).Where(x => x.Isdelete == false).ToList()), pageFilter);
            return customerresponse;
        }

        public List<CustomerListViewModel> GetCustomerViewList(List<Customer> customertable)
        {
            return customertable.Select(c =>
               {
                   CustomerListViewModel model = new()
                   {
                       Id = c.Id,
                       AC = c.Ac,
                       Name = c.Name,
                       PostCode = c.Postcode,
                       Country = c.Country,
                       Telephone = c.Telephone,
                       Relationship = c.Relation,
                       currency = c.Currency
                   };
                   return model;
               }).ToList<CustomerListViewModel>();
        }

        public Customer? GetInfoOfAC(string acno)
        {
            return _db.Customers.FirstOrDefault(x => x.Ac == acno && x.Isdelete == false);
        }

        public Customer? GetInfoOfId(int id)
        {
            return _db.Customers.FirstOrDefault(x => x.Id == id && x.Isdelete == false);
        }
        public List<Group> CustomerGroupDetail(int id, string search)
        {
            if (search == null)
            {
                return _db.Groups.Where(x => x.CustomerId == id && !x.Isdelete).OrderBy(x => x.Name).ToList();
            }
            return _db.Groups.Where(x => x.CustomerId == id && !x.Isdelete)
                .Where(x => x.Name.Trim().ToLower().Contains(search.Trim().ToLower()))
                .OrderBy(x => x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()) != -1 ? x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()) : int.MaxValue)
                .ThenBy(x => x.Name)
                .ToList();
        }

        public List<Group> AllGroups()
        {
            return _db.Groups.ToList();
        }

        public bool AddGroupInCustomer(int customerid, int groupid, string groupname)
        {
            string name = groupname.Trim().ToLower();
            if (groupid != 0)
            {
                Group group = _db.Groups.FirstOrDefault(x=>x.Id==groupid && !x.Isdelete);
                if(group != null)
                {
                    group.Name = groupname;
                    _db.Groups.Update(group);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Group group = _db.Groups.FirstOrDefault(x => x.Name.Trim().ToLower() == name && x.CustomerId == customerid && !x.Isdelete);
                if (group == null)
                {
                    group = new Group
                    {
                        Name = groupname,
                        CustomerId = customerid,
                        Isdelete = false,
                        Isselect = true,
                    };
                    _db.Groups.Add(group);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //if (!_db.Mappings.Any(x => x.CustomerId == customerid && x.GroupId == group.Id))
            //{
            //    _db.Mappings.Add(new Mapping
            //    {
            //        CustomerId = customerid,
            //        GroupId = group.Id,
            //    });
            //    _db.SaveChanges();
            //}
        }

        public void DeleteGroupFromCustomer(int customerid, int groupid)
        {
            //Mapping mapping = _db.Mappings.FirstOrDefault(x => x.CustomerId == customerid && x.GroupId == groupid);
            //if (mapping != null)
            //{
            //    _db.Mappings.Remove(mapping);
            //    _db.SaveChanges();
            //}
            Group group = _db.Groups.FirstOrDefault(x => x.Id == groupid && x.CustomerId == customerid);
            group.Isdelete = true;
            _db.SaveChanges();
        }


        public List<Supplier> GetSupplierOfGroup(int groupid, string search)
        {
            if (search == null)
            {
                return _db.Suppliers.Where(x => x.GroupId == groupid).OrderBy(x => x.Name).ToList();
            }
            return _db.Suppliers.Where(x => x.GroupId == groupid && x.Name.Trim().ToLower().Contains(search.Trim().ToLower()))
                 .OrderBy(x => x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()) != -1 ? x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()) : int.MaxValue)
                .ThenBy(x => x.Name)
                .ToList();
        }

        public List<Supplier> GetNullSupplier(string search)
        {
            if (search == null)
            {
                return _db.Suppliers.Where(x => x.GroupId == null).OrderBy(x => x.Name).ToList();
            }
            return _db.Suppliers.Where(x => x.GroupId == null && x.Name.Trim().ToLower().Contains(search.Trim().ToLower()))
                .OrderBy(x => x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()) != -1 ? x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()) : int.MaxValue)
                .ThenBy(x => x.Name)
                .ToList();
        }

    }
}
