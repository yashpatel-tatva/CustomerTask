using CustomerTask.Services.IServices;
using DataAccess.API;
using DataAccess.SDs;

namespace CustomerTask.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string _url;
        public CustomerService(IHttpClientFactory httpclientfactory, IConfiguration configuration) : base(httpclientfactory)
        {
            _httpClientFactory = httpclientfactory;
            _url = configuration.GetValue<string>("ServiceUrls:CustomerAPI");
        }

        public Task<T> CreateAsync<T>(Customer dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = dto,
                Url = _url + "/api/customerAPI"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Url = _url + "/api/customerAPI/"+id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Url = _url + "/api/customerAPI"
            });
        }

        public Task<T> GetAsync<T>(string acNo)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Url = _url + "/api/customerAPI/"+acNo
            });
        }

        public Task<T> UpdateAsync<T>(Customer dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = dto,
                Url = _url + "/api/customerAPI"
            });
        }
    }
}
