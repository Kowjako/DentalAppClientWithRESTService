using System.Net;
using System.Net.Http;

namespace DentalClientWithRESTService.Models
{
    public class HttpResponseWrapper
    {
        private HttpStatusCode _statusCode;
        
        public string ResponseCode => $"{(int)_statusCode} ({_statusCode.ToString()})";

        public HttpResponseWrapper(HttpResponseMessage response)
        {
            _statusCode = response.StatusCode;  
        }
    }
}
