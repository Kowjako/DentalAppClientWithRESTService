using DentalClientWithRESTService.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Net;
using System.Net.Http;

namespace DentalClientWithRESTService.Models
{
    public class HttpResponseWrapper : INotifyPropertyChanged
    {
        private HttpStatusCode _statusCode;

        public string ResponseCode => $"{(int)_statusCode} ({_statusCode.ToString()})";

        private string _responseMessage;

        public string ResponseMessage
        {
            get => _responseMessage;
            set
            {
                _responseMessage = value;
                OnPropertyChanged(nameof(ResponseMessage));
            }
        }

        public HttpResponseWrapper(HttpResponseMessage response)
        {
            _statusCode = response.StatusCode;

            if (_statusCode == HttpStatusCode.BadRequest)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                _responseMessage = JToken.Parse(content).ToString(Formatting.Indented);
            }
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
