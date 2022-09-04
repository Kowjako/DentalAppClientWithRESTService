using DentalClientWithRESTService.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DentalClientWithRESTService.HTTPClient
{
    public class HttpClientProxy
    {
        private static HttpClientProxy _instance;
        public static HttpClientProxy Instance => _instance ?? (_instance = new HttpClientProxy());

        private HttpClient _client;

        private HttpClientProxy()
        {
            _client = new HttpClient();
        }

        public async Task<List<Clinic>> GetAllClinic()
        {
            var result = await _client.GetAsync("https://localhost:44347/api/clinic");
            var data = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Clinic>>(data);
        }
    }
}
