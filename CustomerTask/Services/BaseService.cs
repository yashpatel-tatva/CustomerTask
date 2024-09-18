using CustomerTask.Models;
using CustomerTask.Services.IServices;
using DataAccess.API;
using DataAccess.SDs;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace CustomerTask.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse respsemodel { get; set; }
        public IHttpClientFactory httpclientfactory { get; set; }
        public BaseService(IHttpClientFactory httpclientfactory)
        {
            this.respsemodel = new APIResponse();
            this.httpclientfactory = httpclientfactory;
        }
        public async Task<T> SendAsync<T>(APIRequest aPIRequest) 
        {
            try
            {
                var client = httpclientfactory.CreateClient("CustomerAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(aPIRequest.Url);
                if (aPIRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(aPIRequest.Data), Encoding.UTF8, "application/json");
                }
                switch (aPIRequest.APIType)
                {
                    case SD.APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    case SD.APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    default: message.Method = HttpMethod.Get; break;
                }
                HttpResponseMessage apiResponse = null;
                apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    Error = new List<string> { Convert.ToString(ex.Message) },
                    Success = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }
    }
}
