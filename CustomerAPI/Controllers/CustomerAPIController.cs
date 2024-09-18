using BusinessAccess.Repository.IRepository;
using CustomerTask;
using CustomerTask.Models;
using DataAccess.DataViewModel;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CustomerAPI.Controllers
{
    [Route("api/customerAPI")]
    [ApiController]
    public class CustomerAPIController : ControllerBase
    {
        private readonly ICustomerRepository _customer;
        protected APIResponse _response;
        public CustomerAPIController(ICustomerRepository customer)
        {
            _customer = customer;
            this._response = new APIResponse();
        }

        [HttpGet]
        [Authorize]
        public ActionResult<APIResponse> GetCustomers()
        {
            _response.Result = _customer.GetCustomerViewList(_customer.GetAll().ToList());
            _response.Success = true;
            _response.HttpStatusCode = HttpStatusCode.OK;
            return _response;
        }

        [HttpGet("{acNo}", Name = "GetCustomer")]
        [Authorize]
        [ResponseCache()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetCustomer(string acNo)
        {
            Customer model = await _customer.GetInfoOfAC(acNo) ?? new Customer();
            if (acNo == "" || acNo == null)
            {
                _response.Success = false;
                _response.HttpStatusCode = HttpStatusCode.BadRequest;
                _response.Error.Add("NOT FOUND");
            }
            else if (model.Id != 0)
            {
                _response.Success = true;
                _response.HttpStatusCode = HttpStatusCode.OK;
                _response.Result = model;
            }
            else
            {
                _response.Success = false;
                _response.HttpStatusCode = HttpStatusCode.NotFound;
                _response.Error.Add("NOT FOUND");

            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetCustomerById")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetCustomerById(int id)
        {
            Customer model = await _customer.GetInfoOfId(id) ?? new Customer();
            if (id == 0|| id == null)
            {
                _response.Success = false;
                _response.HttpStatusCode = HttpStatusCode.BadRequest;
                _response.Error.Add("NOT FOUND");
            }
            else if (model.Id != 0)
            {
                _response.Success = true;
                _response.HttpStatusCode = HttpStatusCode.OK;
                _response.Result = model;
            }
            else
            {
                _response.Success = false;
                _response.HttpStatusCode = HttpStatusCode.NotFound;
                _response.Error.Add("NOT FOUND");

            }
            return _response;
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> CreateCustomer(Customer model)
        {
            List<string> error = new();
            var acNo = "";
            _response.HttpStatusCode = HttpStatusCode.OK;
            if (!ModelState.IsValid)
            {
                _response.Success = false;
                _response.HttpStatusCode = HttpStatusCode.BadRequest;
                _response.Result = model;
            }
            else if (await _customer.CheckACExist(model.Id, model.Ac))
            {
                ModelState.AddModelError("CustomErro", "Ac No Already Exist");
                _response.Success = false;
                _response.HttpStatusCode = HttpStatusCode.BadRequest;
                _response.Result = model;
            }
            else if (await _customer.CheckCompanyNameExist(model.Id, model.Name))
            {
                ModelState.AddModelError("CustomErro", "Name  Already Exist");
                _response.Success = false;
                _response.HttpStatusCode = HttpStatusCode.BadRequest;
                _response.Result = model;
            }
            else
            {
                acNo = model.Ac;
                await _customer.UpSertCustomer(model);
                _response.Success = true;
                _response.Result = model;
            }
            foreach (var er in ModelState)
            {
                error.Add(er.Value.Errors.ElementAt(0).ErrorMessage);
            }
            _response.Error = error;
            return CreatedAtRoute("GetCustomer", new { acNo }, _response);
        }

        [Authorize(Roles = "superadmin")]
        [HttpDelete("{ids}", Name = "DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteCustomer(string ids)
        {
            var idList = ids.Split(',').Select(int.Parse).ToList();
            await _customer.DeleteCustomerlist(idList);
            _response.Success = true;
            _response.HttpStatusCode = HttpStatusCode.OK;
            return _response;
        }
    }
}
