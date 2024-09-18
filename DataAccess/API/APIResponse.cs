using System.Net;

namespace CustomerTask.Models
{
    public class APIResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public List<string> Error { get; set; }   = new List<string>();
        public bool Success { get; set; } = true;
        public object Result { get; set; }
    }
}