using DataAccess.DataModels;

namespace DataAccess.DTOs
{
    public class LoginResponseDTO
    {
        public LocalUser LocalUser { get; set; }
        public string Token { get; set; }
    }
}
