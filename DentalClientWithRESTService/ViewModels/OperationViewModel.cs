using DentalClientWithRESTService.HTTPClient;
using DentalClientWithRESTService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClientWithRESTService.ViewModels
{
    public class OperationViewModel : AbstractViewModelWithSubentity<Clinic, OperationDTO>
    {
        private OperationDTO _selectedOperation;
        public OperationDTO SelectedOperation
        {
            get => _selectedOperation;
            set
            {
                _selectedOperation = value;
                OnPropertyChanged();
            }
        }

        private Clinic _selectedClinic;
        public Clinic SelectedClinic
        {
            get => _selectedClinic;
            set
            {
                if (value == null) return;
                _selectedClinic = value;
                OnPropertyChanged();
                Task.Run(async () => await LoadSubEntities()).Wait();
            }
        }

        public CreateOperationDTO CreateOperation { get; set; } = new CreateOperationDTO();

        public OperationViewModel()
        {
            Task.Run(async () =>
            {
                var response = await HttpClientProxy.Instance.GetAll("clinic");
                PagedData = await HttpClientProxy.Instance.ReadDataAsList<Clinic>(response);
            }).Wait();

            Entities = PagedData.Data.ToList();

            OnPropertyChanged(nameof(Entities));
        }

        #region AbstractViewModelWithSubentity members

        public override string BuildRouteToSubEntitiy()
        {
            return $"clinic/{SelectedClinic.Id}/operation";
        }

        #endregion

        #region Commands

        private RelayCommand _deleteOperation;
        public RelayCommand DeleteOperation =>
            _deleteOperation ??= new RelayCommand(async (e) =>
            {
                var response = await HttpClientProxy.Instance.DeleteById(BuildRouteToSubEntitiy(), SelectedSubEntity.Id);

                HttpResponse = new HttpResponseWrapper(response);

                await RefreshList();
            });

        private RelayCommand _deleteAllOperations;
        public RelayCommand DeleteAllOperations =>
            _deleteAllOperations ??= new RelayCommand(async (e) =>
            {
                var response = await HttpClientProxy.Instance.Delete(BuildRouteToSubEntitiy());

                HttpResponse = new HttpResponseWrapper(response);

                await RefreshList();
            });

        private RelayCommand _addOperation;
        public RelayCommand AddOperation =>
            _addOperation ??= new RelayCommand(async (e) =>
            {
                CreateOperation.ClinicId = SelectedClinic.Id;
                var response = await HttpClientProxy.Instance.Add(BuildRouteToSubEntitiy(), CreateOperation);

                HttpResponse = new HttpResponseWrapper(response);

                await RefreshList();
            });

        #endregion
    }
}
