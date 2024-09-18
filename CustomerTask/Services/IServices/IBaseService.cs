using CustomerTask.Models;
using DataAccess.API;

namespace CustomerTask.Services.IServices
{
    public interface IBaseService
    {
        APIResponse respsemodel { get; set; }

        Task<T> SendAsync<T>(APIRequest aPIRequest);
    }
}
