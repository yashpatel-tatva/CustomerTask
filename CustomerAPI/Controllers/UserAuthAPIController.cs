using Azure;
using BusinessAccess.Repository.IRepository;
using CustomerTask.Models;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Net;

namespace CustomerAPI.Controllers
{
    [Route("api/userauthAPI")]
    [ApiController]
    public class UserAuthAPIController : ControllerBase
    {
        private readonly IUserRepository _userauth;
        protected APIResponse _response;
        public UserAuthAPIController(IUserRepository userRepository)
        {
            _userauth = userRepository;
            this._response = new APIResponse();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            LoginResponseDTO response = await _userauth.Login(loginRequestDTO);
            if (response == null || string.IsNullOrEmpty(response.Token))
            {
                _response.HttpStatusCode = HttpStatusCode.BadRequest;
                _response.Success = false;
                _response.Error.Add("Username or password is incorrect");
                return BadRequest(_response);
            }
            _response.HttpStatusCode = HttpStatusCode.OK;
            _response.Success = true;
            _response.Result = response;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            bool ifUserNameUnique = _userauth.IsuniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                _response.HttpStatusCode = HttpStatusCode.BadRequest;
                _response.Success = false;
                _response.Error.Add("Username already exists");
                return BadRequest(_response);
            }
            var user = await _userauth.Register(model);
            if (user == null)
            {
                _response.HttpStatusCode = HttpStatusCode.BadRequest;
                _response.Success = false;
                _response.Error.Add("Username already exists");
                return BadRequest(_response);
            }

            _response.HttpStatusCode = HttpStatusCode.OK;
            _response.Success = true;
            return Ok(_response);
        }
    }
}
