using DentalClientWithRESTService.HTTPClient;
using DentalClientWithRESTService.Models;
using DentalClinicWithRestService.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DentalClientWithRESTService.ViewModels
{
    public class ClinicViewModel : INotifyPropertyChanged
    {
        public List<Clinic> Clinics { get; set; }

        private Clinic _selectedClinic;
        public Clinic SelectedClinic
        {
            get => _selectedClinic;
            set
            {
                _selectedClinic = value;
                OnPropertyChanged();
            }
        }

        private HttpResponseWrapper _httpResponse;
        public HttpResponseWrapper HttpResponse
        {
            get => _httpResponse;
            set
            {
                _httpResponse = value;
                OnPropertyChanged();
            }
        }

        private PagedResult<Clinic> _pagedData;
        public PagedResult<Clinic> PagedData
        {
            get => _pagedData;
            set
            {
                _pagedData = value;
                OnPropertyChanged(nameof(PagedData));
            }
        }

        public CreateClinicDTO CreateClinic { get; set; } = new CreateClinicDTO();

        public ClinicViewModel()
        {
            Task.Run(async () =>
            {
                var response = await HttpClientProxy.Instance.GetAll("clinic");
                _pagedData = await HttpClientProxy.Instance.ReadDataAsList<Clinic>(response);
            }).Wait();

            Clinics = _pagedData.Data.ToList();

            OnPropertyChanged(nameof(Clinics));
        }

        #region Commands

        private RelayCommand _updateClinic;
        public RelayCommand UpdateClinic =>
            _updateClinic ??= new RelayCommand(async (e) =>
            {
                var updateClinicDTO = new UpdateClinicDTO() 
                { 
                    Name = SelectedClinic.Name, 
                    Location = SelectedClinic.Location 
                };

                var response = await HttpClientProxy.Instance.Update($"clinic/{SelectedClinic.Id}", updateClinicDTO);
                HttpResponse = new HttpResponseWrapper(response);
            });

        private RelayCommand _deleteClinic;
        public RelayCommand DeleteClinic =>
            _deleteClinic ??= new RelayCommand(async (e) =>
            {
                var response = await HttpClientProxy.Instance.DeleteById("clinic", SelectedClinic.Id);

                HttpResponse = new HttpResponseWrapper(response);

                var list = await HttpClientProxy.Instance.GetAll("clinic");
                _pagedData = await HttpClientProxy.Instance.ReadDataAsList<Clinic>(response);
                Clinics = _pagedData.Data.ToList();

                OnPropertyChanged(nameof(Clinics));
            });

        private RelayCommand _addClinic;
        public RelayCommand AddClinic =>
            _addClinic ??= new RelayCommand(async (e) =>
            {
                var response = await HttpClientProxy.Instance.Add("clinic", CreateClinic);

                HttpResponse = new HttpResponseWrapper(response);

                var list = await HttpClientProxy.Instance.GetAll("clinic");
                _pagedData = await HttpClientProxy.Instance.ReadDataAsList<Clinic>(response);
                Clinics = _pagedData.Data.ToList();

                OnPropertyChanged(nameof(Clinics));
            });

        private RelayCommand _nextPage;
        public RelayCommand NextPage =>
            _nextPage ??= new RelayCommand(async (e) =>
            {
                if (_pagedData.PageNumber + 1 > _pagedData.TotalPages) return;
                var response = await HttpClientProxy.Instance.GetAll("clinic", pageNumber: _pagedData.PageNumber + 1);
                _pagedData = await HttpClientProxy.Instance.ReadDataAsList<Clinic>(response);

                Clinics = _pagedData.Data.ToList();

                OnPropertyChanged(nameof(Clinics));
            });

        private RelayCommand _prevPage;
        public RelayCommand PrevPage =>
            _prevPage ??= new RelayCommand(async (e) =>
            {
                if (_pagedData.PageNumber - 1 < 1) return;
                var response = await HttpClientProxy.Instance.GetAll("clinic", pageNumber: _pagedData.PageNumber - 1);
                _pagedData = await HttpClientProxy.Instance.ReadDataAsList<Clinic>(response);

                Clinics = _pagedData.Data.ToList();

                OnPropertyChanged(nameof(Clinics));
            });

        #endregion

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
