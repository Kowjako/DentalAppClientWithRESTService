using DentalClientWithRESTService.HTTPClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DentalClientWithRESTService.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public string Login { get; set; }
        public string Password { get; set; }

        private string _responseMessage;
        public string ResponseMessage
        {
            get => _responseMessage;
            private set
            {
                _responseMessage = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _performLogin;

        public RelayCommand PerformLogin =>
            _performLogin ??= new RelayCommand(async (e) =>
            {
                var response = await HttpClientProxy.Instance.Login(Login, Password);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpClientProxy.Instance.JwtToken = await response.Content.ReadAsStringAsync();
                    ResponseMessage = "OK";
                }
                else
                {
                    ResponseMessage = "Logowanie się nie powiodło";
                }
            });

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        #endregion
    }
}
