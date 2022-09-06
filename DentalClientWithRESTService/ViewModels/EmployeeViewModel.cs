using DentalClientWithRESTService.HTTPClient;
using DentalClientWithRESTService.Models;
using DentalClinicWithRestService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClientWithRESTService.ViewModels
{
    public class EmployeeViewModel : AbstractViewModel<EmployeeDTO>
    {
        private EmployeeDTO _selectedEmployee;
        public EmployeeDTO SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
            }
        }

        public CreateEmployeeDTO CreateEmployee { get; set; } = new CreateEmployeeDTO();

        public EmployeeViewModel()
        {
            Task.Run(async () =>
            {
                var response = await HttpClientProxy.Instance.GetAll("employee");
                PagedData = await HttpClientProxy.Instance.ReadDataAsList<EmployeeDTO>(response);
            }).Wait();

            Entities = PagedData.Data.ToList();

            OnPropertyChanged(nameof(Entities));
        }

        #region Commands

        private RelayCommand _updateEmployee;
        public RelayCommand UpdateEmployee =>
            _updateEmployee ??= new RelayCommand(async (e) =>
            {
                var updateClinicDTO = new UpdateEmployeeDTO()
                {
                    Name = SelectedEmployee.Name,
                    Surname = SelectedEmployee.Surname,
                    Phone = SelectedEmployee.Phone,
                    Email = SelectedEmployee.Email
                };

                var response = await HttpClientProxy.Instance.Update($"employee/{SelectedEmployee.Id}", updateClinicDTO);
                HttpResponse = new HttpResponseWrapper(response);
            });

        private RelayCommand _deleteEmployee;
        public RelayCommand DeleteEmployee =>
            _deleteEmployee ??= new RelayCommand(async (e) =>
            {
                var response = await HttpClientProxy.Instance.DeleteById("employee", SelectedEmployee.Id);

                HttpResponse = new HttpResponseWrapper(response);

                var list = await HttpClientProxy.Instance.GetAll("employee");
                PagedData = await HttpClientProxy.Instance.ReadDataAsList<EmployeeDTO>(response);
                Entities = PagedData.Data.ToList();

                OnPropertyChanged(nameof(Entities));
            });

        private RelayCommand _addEmployee;
        public RelayCommand AddEmployee =>
            _addEmployee ??= new RelayCommand(async (e) =>
            {
                var response = await HttpClientProxy.Instance.Add("employee", CreateEmployee);

                HttpResponse = new HttpResponseWrapper(response);

                var list = await HttpClientProxy.Instance.GetAll("employee");
                PagedData = await HttpClientProxy.Instance.ReadDataAsList<EmployeeDTO>(response);
                Entities = PagedData.Data.ToList();

                OnPropertyChanged(nameof(Entities));
            });

        #endregion

        
    }
}
