using BusinessAccess.Repository.IRepository;
using CustomerTask.Models;
using CustomerTask.Services.IServices;
using DataAccess.DataViewModel;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CustomerTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerRepository _customer;
        private readonly IGroupRepository _group;
        private readonly ICustomerService _customerService;

        public HomeController(ILogger<HomeController> logger, ICustomerRepository customerRepository, IGroupRepository Group, ICustomerService customerService)
        {
            _logger = logger;
            _customer = customerRepository;
            _group = Group;
            _customerService = customerService;
        }

        public IActionResult Index(PageFilterRequestDTO<CustomerSearchFilterDTO> pageFilterDTO)
        {
            ViewBag.search = pageFilterDTO.Search;
            ViewBag.Currentpage = pageFilterDTO.currentpage;
            ViewBag.PageSize = pageFilterDTO.pagesize;
            return View();
        }

        public async Task<IActionResult> DetailOfCustomer(string acNo)
        {
            return View("DetailOfCustomer", await _customer.GetGroupCountandName(acNo));
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

        public Task<List<string>> AccountNoDrop()
        {
            return _customer.GetAllAcoountNumber();
        }

        public Task<Customer> GetInfoOfAC(string acNo)
        {
            return _customer.GetInfoOfAC(acNo);
        }

        //public async Task UpSertCustomer(CustomerDetailViewModel customer)
        //{
        //    Customer model = _customer.GetTableCustomer(customer);
        //    List<string?> issuscribe = Request.Form["IssubscribeInvoice"].ToList();
        //    if (issuscribe.Count == 0) model.Issubscribe = false;
        //    else if (issuscribe[0] == "Subscribed") model.Issubscribe = true;
        //    else model.Issubscribe = false;
        //    await _customer.UpSertCustomer(model);
        //}

        public async Task<IActionResult> UpSertCustomer(CustomerDetailViewModel customer)
        {
            Customer model = _customer.GetTableCustomer(customer);
            List<string?> issuscribe = Request.Form["IssubscribeInvoice"].ToList();
            if (issuscribe.Count == 0) model.Issubscribe = false;
            else if (issuscribe[0] == "Subscribed") model.Issubscribe = true;
            else model.Issubscribe = false;
            var response = await _customerService.CreateAsync<APIResponse>(model);
            if (response != null && response.Success) return Ok(response.Result);
            else return BadRequest();
        }

        public void DeleteCustomerlist(List<int> ids)
        {
            _customer.DeleteCustomerlist(ids);
        }

        public Task<bool> CheckACExist(int id, string acNo)
        {
            return _customer.CheckACExist(id, acNo);
        }

        public Task<bool> CheckCompanyNameExist(int id, string name)
        {
            return _customer.CheckCompanyNameExist(id, name);
        }

        public async Task<IActionResult> OpenDetailForm(int id)
        {
            return View("_DetailForm", _customer.GetCustomerModel(await _customer.GetInfoOfId(id) ?? new Customer()));
        }

        public IActionResult OpenGroupModal(int id, bool isEdit)
        {
            return PartialView("EditGroupModal", new { customerId = id, isEdit });
        }

        public async Task<IActionResult> CustomerGroupData(int customerId, string search, bool isEdit)
        {
            ViewBag.isEdit = isEdit;
            return PartialView("GroupListForCustomer", await _customer.CustomerGroupDetail(customerId, search));
        }

        public IActionResult AddGroupModal(int customerId, int groupId)
        {
            return PartialView("AddGroupModal", _group.GetCustomerGroupModel(customerId, groupId));
        }

        public async Task<bool> SelectGroupInCustomer(int groupId, bool isSelect)
        {
            return await _group.SelectGroupInCustomer(groupId, isSelect);
        }
        public async Task<bool> UnSelectAllGroupInCustomer(int customerId)
        {
            return await _group.UnSelectAllGroupInCustomer(customerId);
        }

        public async Task<IActionResult> UpSertCustomerGroup(int customerId, int groupId, string group)
        {
            await _customer.UpSertCustomerGroup(customerId, groupId, group);
            return RedirectToAction("OpenGroupModal", new { id = customerId, isEdit = true });
        }

        public async Task<bool> CustomerGroupExist(int customerId, int groupId, string group)
        {
            return await _customer.CustomerGroupExist(customerId, groupId, group);
        }

        public async Task<IActionResult> DeleteGroupCustomer(int customerId, int groupId)
        {
            await _customer.DeleteGroupCustomer(customerId, groupId);
            return RedirectToAction("OpenGroupModal", new { id = customerId, isEdit = true });
        }

        public async Task<int> UpSertCustomerContact(Contact contact)
        {
            return await _customer.UpSertCustomerContact(contact);
        }

        public async Task<ContactViewModel> GetContactDataById(int contactId)
        {
            return await _customer.GetContactDataById(contactId);
        }

        public async Task<IActionResult> GetContactListCustomer(int customerId, string search)
        {
            return PartialView("ContactListForCustomer", await _customer.GetContactListCustomer(customerId, search));
        }

        public async Task DeletthisContact(int contactId)
        {
            await _customer.DeletthisContact(contactId);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}