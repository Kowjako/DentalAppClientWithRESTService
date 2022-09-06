using DentalClinicWithRestService.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DentalClientWithRESTService.HTTPClient
{
    public class HttpClientProxy
    {
        private const string _baseUrl = "https://localhost:44347/api/";

        private static HttpClientProxy _instance;
        public static HttpClientProxy Instance => _instance ?? (_instance = new HttpClientProxy());

        private HttpClient _client;

        private HttpClientProxy()
        {
            _client = new HttpClient();
        }

        #region GET/POST/PUT/DELETE

        // GET
        public async Task<HttpResponseMessage> GetAll(string url, string sortDirection = "ASC",
                                                      string sortBy = "", int pageNumber = 1,
                                                      string searchPhrase = "")
        {

            return await _client.GetAsync(_baseUrl + url + GenerateQueryParameters(new Dictionary<string, string>()
            {
                { "sortDirection", sortDirection },
                { "sortBy", sortBy },
                { "pageNumber", pageNumber.ToString() },
                { "searchPhrase", searchPhrase }
            }));
        }

        // GET
        public async Task<HttpResponseMessage> GetById(string url, int id)
        {
            return await _client.GetAsync(_baseUrl + url + "/" + id);
        }

        // DELETE
        public async Task<HttpResponseMessage> DeleteById(string url, int id)
        {
            return await _client.DeleteAsync(_baseUrl + url + "/" + id);
        }

        // PUT
        public async Task<HttpResponseMessage> Update<T>(string url, T data)
        {
            var jsonData = JsonConvert.SerializeObject(data);

            var requestContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            return await _client.PutAsync(_baseUrl + url, requestContent);
        }

        // POST
        public async Task<HttpResponseMessage> Add<T>(string url, T data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var requestContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            return await _client.PostAsync(_baseUrl + url, requestContent);
        }

        #endregion

        #region Common

        public async Task<PagedResult<T>> ReadDataAsList<T>(HttpResponseMessage msg)
        {
            var data = await msg.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PagedResult<T>>(data);
        }

        #endregion

        #region Private members

        private string GenerateQueryParameters(Dictionary<string, string> param)
        {
            var sb = new StringBuilder();
            sb.Append("?");
            foreach (var k in param.Keys)
            {
                if (!string.IsNullOrEmpty(param[k]))
                {
                    sb.Append(k + "=" + param[k]);
                    sb.Append("&");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        #endregion


    }
}
