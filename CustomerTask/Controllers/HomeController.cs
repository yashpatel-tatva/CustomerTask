using BusinessAccess.Repository.IRepository;
using CustomerTask.Models;
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


        public HomeController(ILogger<HomeController> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customer = customerRepository;
        }

        public IActionResult Index(PageFilterRequestDTO<CustomerSearchFilterDTO> pageFilterDTO)
        {
            ViewBag.Search = pageFilterDTO.search;
            ViewBag.Currentpage = pageFilterDTO.currentpage;
            ViewBag.PageSize = pageFilterDTO.pagesize;
            //ViewBag.acsort = pageFilterDTO.acsort;
            //ViewBag.namesort = pageFilterDTO.namesort;
            //ViewBag.pcsort = pageFilterDTO.pcsort;
            //ViewBag.cntrysort = pageFilterDTO.cntrysort;
            //ViewBag.tpsort = pageFilterDTO.tpsort;
            //ViewBag.rlsnsort = pageFilterDTO.rlsnsort;
            //ViewBag.currsort = pageFilterDTO.currsort;
            return View();
        }

        public IActionResult DetailOfCustomer(string acno)
        {
            return View("DetailOfCustomer", acno);
        }

        //public int GetCustomerCount(string search)
        //{
        //    return _customer.GetCustomerCount(search);
        //}

        public PageFilterResponseDTO<CustomerListViewModel> GetCustomerList(PageFilterRequestDTO<CustomerSearchFilterDTO> pageFilterDTO)
        {
            PageFilterResponseDTO<CustomerListViewModel> model = _customer.GetCustomerList(pageFilterDTO);
            return model;
        }

        [HttpPost]
        public IActionResult GetCustomerListView([FromBody]PageFilterRequestDTO<CustomerSearchFilterDTO> pageFilterDTO)
        {
            PageFilterResponseDTO<CustomerListViewModel> model = GetCustomerList(pageFilterDTO);
            int pagncount = model.TotalPage;
            int totalrecords = model.TotalRecords;
            return PartialView("ListView" , model);
        }

        public List<string> AccountNoDrop()
        {
            return _customer.GetAllAcoountNumber();
        }

        public Customer GetInfoOfAC(string acno)
        {
            Customer customer = _customer.GetInfoOfAC(acno);

            return customer;

        }

        public void AddCustomer(CustomerDetailViewModel customer)
        {
            Customer model = new Customer();
            model.Name = customer.Name;
            model.Postcode = customer.Postcode;
            model.Country = customer.Country;
            model.Telephone = customer.Telephone;
            model.Relation = customer.Relation;
            model.Currency = customer.Currency;
            model.Address1 = customer.Address1;
            model.Address2 = customer.Address2;
            model.Town = customer.Town;
            model.County = customer.County;
            model.Email = customer.Email;
            model.Ac = customer.Ac;
            model.Id = customer.Id;
            model.Isdelete = false;


            List<string?> issuscribe = Request.Form["Issubscribe"].ToList();
            if (issuscribe.Count != 0)
            {
                model.Issubscribe = true;
            }
            else
            {
                model.Issubscribe = false;
            }
            if (customer.Id == 0)
            {
                _customer.Addthis(model);
            }
            else
            {
                _customer.Editthis(model);
            }

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
                CustomerDetailViewModel model = new CustomerDetailViewModel();
                model.Name = customer.Name;
                model.Postcode = customer.Postcode;
                model.Country = customer.Country;
                model.Telephone = customer.Telephone;
                model.Relation = customer.Relation;
                model.Currency = customer.Currency;
                model.Address1 = customer.Address1;
                model.Address2 = customer.Address2;
                model.Town = customer.Town;
                model.County = customer.County;
                model.Email = customer.Email;
                model.Issubscribe = (bool)customer.Issubscribe;
                model.Ac = customer.Ac;
                model.Id = customer.Id;
                return PartialView("_DetailForm", model);
            }
        }

        private Customer GetInfoOfId(int id)
        {
            return _customer.GetInfoOfId(id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}