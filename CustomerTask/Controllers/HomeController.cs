using BusinessAccess.Repository.IRepository;
using CustomerTask.Models;
using DataAccess.DataViewModel;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace CustomerTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerRepository _customer;
        private readonly IGroupRepository _group;


        public HomeController(ILogger<HomeController> logger, ICustomerRepository customerRepository, IGroupRepository group)
        {
            _logger = logger;
            _customer = customerRepository;
            _group = group;
        }

        public IActionResult Index(PageFilterRequestDTO<CustomerSearchFilterDTO> pageFilterDTO)
        {
            ViewBag.Search = pageFilterDTO.search;
            ViewBag.Currentpage = pageFilterDTO.currentpage;
            ViewBag.PageSize = pageFilterDTO.pagesize;
            return View();
        }

        public IActionResult DetailOfCustomer(string acno)
        {
            return View("DetailOfCustomer", _customer.GetGroupCountandName(acno));
        }

        public PageFilterResponseDTO<CustomerListViewModel> GetCustomerList(PageFilterRequestDTO<CustomerSearchFilterDTO> pageFilterDTO)
        {
            return _customer.GetCustomerList(pageFilterDTO);
        }

        [HttpPost]
        public IActionResult GetCustomerListView([FromBody] PageFilterRequestDTO<CustomerSearchFilterDTO> pageFilterDTO)
        {
            return PartialView("ListView", GetCustomerList(pageFilterDTO));
        }

        public List<string> AccountNoDrop()
        {
            return _customer.GetAllAcoountNumber();
        }

        public Customer GetInfoOfAC(string acno)
        {
            return _customer.GetInfoOfAC(acno);
        }

        public IActionResult AddCustomer(CustomerDetailViewModel customer)
        {
            //if(false)
            //{
            //    return BadRequest("Not Valid Form");
            //}
            Customer model = new Customer
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
            List<string?> issuscribe = Request.Form["Issubscribe"].ToList();
            if (issuscribe[0] == "Subscribed") model.Issubscribe = true;
            else model.Issubscribe = false;
            //if (customer.Id == 0) _customer.Addthis(model);
            //else _customer.Editthis(model);

            _customer.Addthis(model);
            
            return Ok("");
        }


        public void DeleteCustomer(string acno)
        {
            _customer.DeleteCustomer(acno);
        }

        public void DeleteCustomerlist(List<int> ids)
        {
            _customer.DeleteCustomerlist(ids);
        }


        public bool CheckACExist(int id, string acno)
        {
            return _customer.CheckACExist(id, acno);
        }

        public bool CheckCompanyNameExist(int id, string name)
        {
            return _customer.CheckCompanyNameExist(id, name);
        }


        public IActionResult OpenDetailForm(int id)
        {
            if (id == 0)
            {
                CustomerDetailViewModel customer = new CustomerDetailViewModel();
                return PartialView("_DetailForm", customer);
            }
            else
            {
                Customer customer = GetInfoOfId(id);
                CustomerDetailViewModel model = new CustomerDetailViewModel
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
                    Issubscribe = (bool)customer.Issubscribe,
                    Ac = customer.Ac,
                    Id = customer.Id
                };
                return PartialView("_DetailForm", model);
            }
        }

        private Customer GetInfoOfId(int id)
        {
            return _customer.GetInfoOfId(id);
        }


        public IActionResult OpenGroupModal(int id)
        {

            return PartialView("EditGroupModal", new { Customerid = id });
        }

        public IActionResult CustomerGroupData(int customerid, string search)
        {
            return PartialView("GroupListForCustomer", _customer.CustomerGroupDetail(customerid, search));
        }

        public IActionResult AddGroupModal(int customerid, int groupid)
        {
            Group g = _group.GetFirstOrDefault(x => x.Id == groupid);
            CustomerGroupViewModel customerGroup = new CustomerGroupViewModel();

            customerGroup.CustomerId = customerid;
            customerGroup.GroupId = groupid;
            customerGroup.GroupName = g != null ?  g.Name : "";
            customerGroup.Groups = null;

            return PartialView("AddGroupModal", customerGroup);
        }

        public bool SelectGroupInCustomer(int groupid , bool isselect)
        {
            return _group.SelectGroupInCustomer(groupid, isselect);
        }
        public bool UnSelectAllGroupInCustomer(int customerid)
        {
            return _group.UnSelectAllGroupInCustomer(customerid);
        }

        public IActionResult AddGroupInCustomer(int customerid, int groupid, string group)
        {
            _customer.AddGroupInCustomer(customerid, groupid, group);
            return RedirectToAction("OpenGroupModal", new { id = customerid });
        }


        public IActionResult DeleteGroupFromCustomer(int customerid, int groupid)
        {
            _customer.DeleteGroupFromCustomer(customerid, groupid);
            return RedirectToAction("OpenGroupModal", new { id = customerid });
        }


        public IActionResult GroupSupplierModel(int groupid, string search)
        {
            GroupAndSuppliersViewModel model = new GroupAndSuppliersViewModel
            {
                GroupId = groupid,
                GroupName = _group.GetFirstOrDefault(x => x.Id == groupid).Name,
                SupplierOfGroup = _customer.GetSupplierOfGroup(groupid, search),
                NullSupplier = _customer.GetNullSupplier(search)
            };
            return PartialView("SupplierFromGroup", model);
        }

        public void EditSupplierinGroup(int groupid, List<int> removefromgroup, List<int> addtogroup)
        {
            _group.EditSupplierinGroup(groupid, removefromgroup, addtogroup);
        }


        public int AddThisContact(Contact contact)
        {
            return _customer.AddThisContact(contact);
        }

        public Contact GetContactDataById(int contactid)
        {
            return _customer.GetContactDataById(contactid);
        }

        public IActionResult GetContactListOfCustomer(int CustomerId, string search)
        {
            List<Contact> model = _customer.GetContactListOfCustomer(CustomerId , search);
            return PartialView("ContactListForCustomer", model);
        }
        public void DeletthisContact(int contactid)
        {
            _customer.DeletthisContact(contactid);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}