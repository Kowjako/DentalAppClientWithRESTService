using DentalClientWithRESTService.HTTPClient;
using DentalClientWithRESTService.Models;
using DentalClinicWithRestService.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClientWithRESTService.ViewModels
{
    public class ClinicViewModel : AbstractViewModel<Clinic>
    { 
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

        public CreateClinicDTO CreateClinic { get; set; } = new CreateClinicDTO();

        public ClinicViewModel()
        {
            Task.Run(async () =>
            {
                var response = await HttpClientProxy.Instance.GetAll("clinic");
                PagedData = await HttpClientProxy.Instance.ReadDataAsList<Clinic>(response);
            }).Wait();

            Entities = PagedData.Data.ToList();

            OnPropertyChanged(nameof(Entities));
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

                await RefreshList();
            });

        private RelayCommand _addClinic;
        public RelayCommand AddClinic =>
            _addClinic ??= new RelayCommand(async (e) =>
            {
                var response = await HttpClientProxy.Instance.Add("clinic", CreateClinic);

                HttpResponse = new HttpResponseWrapper(response);

                await RefreshList();
            });

        #endregion
    }
}
