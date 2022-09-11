using DentalClientWithRESTService.HTTPClient;
using DentalClientWithRESTService.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DentalClientWithRESTService.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        public RegisterUserDTO UserData { get; set; } = new RegisterUserDTO();

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

        private RelayCommand _registerUser;
        public RelayCommand RegisterUser =>
            _registerUser ??= new RelayCommand(async (e) =>
            {
                UserData.RoleId++; // bo index Combobox startuje od 0
                var response = await HttpClientProxy.Instance.Add("account/register", UserData);
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ResponseMessage = "OK";
                }
                else
                {
                    ResponseMessage = "Rejestracja się nie powiodła";
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
