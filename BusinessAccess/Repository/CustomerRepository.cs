using BusinessAccess.Repository.IRepository;
using CustomerTask;
using DataAccess.DataViewModel;
using DataAccess.DTOs;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Group = CustomerTask.Group;

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

        public async Task UpSertCustomer(Customer model)
        {
            model.Isdelete = false;
            if (model.Id == 0) await _db.Customers.AddAsync(model);
            else _db.Customers.Update(model);
            if (model.Email != null)
                await _db.Contacts.Where(x => x.CustomerId == model.Id && !x.Isdelete && x.Email == model.Email).ForEachAsync(x =>
               {
                   x.MailingList = model.Issubscribe ?? false ? "Subscribed" : "Unsubscribed";
               });
            await _db.SaveChangesAsync();
        }

        public async Task<bool> CheckACExist(int id, string acNo)
        {
            return await _db.Customers.AnyAsync(x => x.Ac == acNo && x.Isdelete == false && x.Id != id);
        }

        public async Task<bool> CheckCompanyNameExist(int id, string name)
        {
            return !string.IsNullOrEmpty(name) && await _db.Customers.AnyAsync(x => x.Name.Trim().ToLower() == name.Trim().ToLower() && x.Isdelete == false && x.Id != id);
        }

        public async Task DeleteCustomerlist(List<int> ids)
        {
            _db.Customers.Where(x => ids.Contains(x.Id) && (x.Isdelete == false)).ToList().ForEach(x => { x.Isdelete = true; });
            _db.Contacts.Where(x => ids.Contains(x.CustomerId ?? 0) && (x.Isdelete == false)).ToList().ForEach(x => { x.Isdelete = true; });
            _db.Groups.Where(x => ids.Contains(x.CustomerId ?? 0) && (x.Isdelete == false)).ToList().ForEach(x => { x.Isdelete = true; });
            await _db.SaveChangesAsync();
        }

        public async Task EditCustomer(Customer model)
        {
            _db.Customers.Update(model);
            await _db.SaveChangesAsync();
        }

        public async Task<List<string>> GetAllAcoountNumber()
        {
            return await _db.Customers.Where(x => x.Isdelete == false).Select(x => x.Ac).ToListAsync();
        }

        public PageFilterResponseDTO<CustomerListViewModel> GetCustomerList(PageFilterRequestDTO<CustomerSearchFilterDTO> pageFilter)
        {
            PageFilterResponseDTO<CustomerListViewModel> customerresponse = _paginationRepository
                .GetPagedData(GetCustomerViewList(_db.Customers.Where(x => x.Isdelete == false).OrderBy(x => x.Ac).ToList()), pageFilter);
            return customerresponse;
        }

        public List<CustomerListViewModel> GetCustomerViewList(List<Customer> customers)
        {
            return customers.Select(c =>
               {
                   CustomerListViewModel model = new()
                   {
                       Id = c.Id,
                       AC = c.Ac,
                       Name = c.Name,
                       PostCode = c.Postcode ?? string.Empty,
                       Country = c.Country ?? string.Empty,
                       Telephone = c.Telephone ?? string.Empty,
                       Relationship = c.Relation ?? string.Empty,
                       currency = c.Currency ?? string.Empty
                   };
                   return model;
               }).ToList();
        }

        public async Task<Customer> GetInfoOfAC(string acNo)
        {
            return await _db.Customers.FirstOrDefaultAsync(x => x.Ac == acNo && x.Isdelete == false) ?? new Customer();
        }

        public async Task<Customer?> GetInfoOfId(int id)
        {
            return await _db.Customers.FirstOrDefaultAsync(x => x.Id == id && x.Isdelete == false) ?? new Customer();
        }

        public async Task<List<GroupViewModel>> CustomerGroupDetail(int id, string search)
        {
            return await _db.Groups
                    .Where(x => x.CustomerId == id && !x.Isdelete)
                    .Where(x => search == null || x.Name.Trim().ToLower().Contains(search.Trim().ToLower()))
                    .OrderBy(x => search == null ? x.Name : (x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()) != -1 ? x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()).ToString() : int.MaxValue.ToString()))
                    .ThenBy(x => x.Name)
                    .Select(group => GetGroupViewModel(group)).ToListAsync();
        }

        public async Task<List<Group>> AllGroups()
        {
            return await _db.Groups.ToListAsync();
        }

        public async Task<bool> CustomerGroupExist(int customerId, int groupId, string groupName)
        {
            return await _db.Groups
               .AnyAsync(x => x.CustomerId == customerId && ((groupId == 0) || (x.Id != groupId)) && !x.Isdelete && x.Name.Trim().ToLower() == groupName.Trim().ToLower());
        }

        public async Task<bool> UpSertCustomerGroup(int customerId, int groupId, string groupName)
        {
            string name = groupName.Trim().ToLower();
            Group group;
            if (groupId != 0)
            {
                group = await _db.Groups.FirstOrDefaultAsync(x => x.Id == groupId && !x.Isdelete) ?? new Group();
                if (group.Id == 0)
                    return false;
                group.Name = groupName;
            }
            else
            {
                group = await _db.Groups.FirstOrDefaultAsync(x => x.Name.Trim().ToLower() == name && x.CustomerId == customerId && !x.Isdelete) ?? new Group();
                if (group.Id != 0)
                    return false;
                group = new Group
                {
                    Name = groupName,
                    CustomerId = customerId,
                    Isdelete = false,
                    Isselect = true,
                };
                _db.Groups.Add(group);
            }
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteGroupCustomer(int customerId, int groupId)
        {
            Group group = await _db.Groups.FirstOrDefaultAsync(x => x.Id == groupId && x.CustomerId == customerId) ?? new Group();
            if (group.Id != 0)
            {
                group.Isdelete = true;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Supplier>> GetSupplierOfGroup(int groupId, string search)
        {
            return await _db.Suppliers.Where(x => x.GroupId == groupId && (search == null || x.Name.Trim().ToLower().Contains(search.Trim().ToLower())))
                .OrderBy(x => search == null ? x.Name : (x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()) != -1 ? x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()).ToString() : int.MaxValue.ToString()))
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Supplier>> GetNullSupplier(string search)
        {
            return await _db.Suppliers.Where(x => x.GroupId == null && (search == null || x.Name.Trim().ToLower().Contains(search.Trim().ToLower())))
                .OrderBy(x => search == null ? x.Name : (x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()) != -1 ? x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()).ToString() : int.MaxValue.ToString()))
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<CustomerGroupViewModel> GetGroupCountandName(string acNo)
        {
            Customer c = await _db.Customers.Include(x => x.Groups).Include(x => x.Contacts).FirstOrDefaultAsync(x => x.Ac == acNo && !(x.Isdelete ?? false)) ?? new();
            CustomerGroupViewModel model = new()
            {
                CustomerId = c.Id,
                Ac = c.Ac,
                Groups = c.Groups.Where(z => !z.Isdelete).ToList(),
                TotalSelectedGroup = c.Groups.Where(x => x.Isselect && !x.Isdelete).Count(),
                Contacts = c.Contacts.Where(x => !x.Isdelete).ToList()
            };
            Group g = c.Groups.FirstOrDefault(x => x.Isselect && !x.Isdelete) ?? new Group();
            model.GroupName = g != null ? g.Name : "Select Group";
            model.GroupId = g != null ? g.Id : 0;
            return model;
        }

        public async Task<int> UpSertCustomerContact(Contact contact)
        {
            Customer c = await _db.Customers.FirstOrDefaultAsync(x => x.Id == contact.CustomerId && x.Email == contact.Email) ?? new Customer();
            if (c.Email != null)
            {
                if (contact.MailingList == "Subscribed") c.Issubscribe = true;
                else if (contact.MailingList == "Unsubscribed") c.Issubscribe = false;
                else c.Issubscribe = null;
            }
            if (contact.Id == 0) _db.Add(contact);
            else _db.Update(contact);
            await _db.Contacts.Where(x => x.CustomerId == contact.CustomerId && !x.Isdelete && x.Email == contact.Email).ForEachAsync(x => { x.MailingList = contact.MailingList; });
            await _db.SaveChangesAsync();
            return contact.Id;
        }

        public async Task<ContactViewModel> GetContactDataById(int contactId)
        {
            return GetContactModel(await _db.Contacts.FirstOrDefaultAsync(x => x.Id == contactId && !x.Isdelete) ?? new Contact());
        }

        public async Task<List<ContactViewModel>> GetContactListCustomer(int customerId, string search)
        {
            return await _db.Contacts.Where(x => !x.Isdelete && x.CustomerId == customerId && (search == null || x.Name.Trim().ToLower().Contains(search.Trim().ToLower())))
                .OrderBy(x => search == null ? x.Name : (x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()) != -1 ? x.Name.Trim().ToLower().IndexOf(search.Trim().ToLower()).ToString() : int.MaxValue.ToString()))
                .ThenBy(x => x.Name)
                .Select(contact => GetContactModel(contact)).ToListAsync();
        }

        public async Task DeletthisContact(int contactId)
        {
            Contact contact = await _db.Contacts.FirstOrDefaultAsync(x => x.Id == contactId && !x.Isdelete) ?? new Contact();
            if (contact.Id != 0) contact.Isdelete = true;
            _db.SaveChanges();
        }

        public CustomerDetailViewModel GetCustomerModel(Customer customer)
        {
            return new()
            {
                Name = customer.Name,
                Postcode = customer.Postcode ?? string.Empty,
                Country = customer.Country ?? string.Empty,
                Telephone = customer.Telephone ?? string.Empty,
                Relation = customer.Relation ?? string.Empty,
                Currency = customer.Currency ?? string.Empty,
                Address1 = customer.Address1 ?? string.Empty,
                Address2 = customer.Address2 ?? string.Empty,
                Town = customer.Town ?? string.Empty,
                County = customer.County ?? string.Empty,
                Email = customer.Email ?? string.Empty,
                Issubscribe = customer.Issubscribe,
                Ac = customer.Ac ?? string.Empty,
                Id = customer.Id
            };
        }

        public Customer GetTableCustomer(CustomerDetailViewModel customer)
        {
            return new()
            {
                Name = customer.Name,
                Postcode = customer.Postcode,
                Country = customer.Country,
                Telephone = customer.Telephone,
                Relation = customer.Relation,
                Currency = customer.Currency,
                Address1 = customer.Address1,
                Address2 = customer.Address2,
                Town = customer.Town,
                County = customer.County,
                Email = customer.Email,
                Ac = customer.Ac,
                Id = customer.Id,
                Isdelete = false
            };
        }

        private static ContactViewModel GetContactModel(Contact contact)
        {
            return new()
            {
                Id = contact.Id,
                Username = contact.Username,
                Name = contact.Name,
                Telephone = contact.Telephone,
                Email = contact.Email,
                MailingList = contact.MailingList,
                Isdelete = contact.Isdelete,
                CustomerId = contact.CustomerId,
            };
        }

        private static GroupViewModel GetGroupViewModel(Group group)
        {
            return new()
            {
                Id = group.Id,
                Name = group.Name,
                Isdelete = group.Isdelete,
                Isselect = group.Isselect,
                CustomerId = group.CustomerId,
            };
        }
    }
}