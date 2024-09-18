using BusinessAccess.Repository.IRepository;
using CustomerTask;
using DataAccess.DataModels;
using DataAccess.DTOs;
using DataAccess.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessAccess.Repository
{
    public class UserRepository :  IUserRepository
    {
        private string secretkey;
        private readonly CustomerDbContext _db;
        public UserRepository(IConfiguration configuration, CustomerDbContext db)
        {
            _db = db;
            secretkey = configuration.GetValue<string>("ApiSetting:Secret");
        }

        public bool IsuniqueUser(string username)
        {
            return !_db.LocalUsers.Any(x => x.UserName == username);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            LocalUser user = _db.LocalUsers.FirstOrDefault(x => x.UserName == loginRequestDTO.Username && x.Password == loginRequestDTO.Password);
            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    LocalUser = null,
                    Token = ""
                };
            }
            var tokenHandler= new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretkey);
            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name , user.Id.ToString()),
                    new Claim(ClaimTypes.Role , user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescripter);
            return new LoginResponseDTO
            {
                Token = tokenHandler.WriteToken(token),
                LocalUser = user,
            };
        }

        public async Task<LocalUser> Register(RegisterRequestDTO registerRequestDTO)
        {
            LocalUser local = new()
            {
                UserName = registerRequestDTO.UserName,
                Name = registerRequestDTO.Name,
                Password = registerRequestDTO.Password,
                Role = registerRequestDTO.Role,
            };
            _db.LocalUsers.Add(local);
            await _db.SaveChangesAsync();
            local.Password = "";
            return local;
        }
    }
}
