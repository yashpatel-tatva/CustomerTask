using static DataAccess.SDs.SD;

namespace DataAccess.API
{
    public class APIRequest
    {
        public APIType APIType { get; set; } = APIType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
