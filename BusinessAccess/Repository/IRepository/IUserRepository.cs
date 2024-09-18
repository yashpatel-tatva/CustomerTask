using DataAccess.DataModels;
using DataAccess.DTOs;
using DataAccess.Repository.IRepository;

namespace BusinessAccess.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsuniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register(RegisterRequestDTO registerRequestDTO);
    }
}
