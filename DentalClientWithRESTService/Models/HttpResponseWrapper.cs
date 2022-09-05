using DentalClientWithRESTService.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;

namespace DentalClientWithRESTService.Models
{
    public class HttpResponseWrapper
    {
        private HttpStatusCode _statusCode;

        public string ResponseCode => $"{(int)_statusCode} ({_statusCode.ToString()})";

        public string ResponseMessage;

        public HttpResponseWrapper(HttpResponseMessage response)
        {
            _statusCode = response.StatusCode;

            if (_statusCode == HttpStatusCode.BadRequest)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                ResponseMessage = JToken.Parse(content).ToString(Formatting.Indented);
            }
        }
    }
}
