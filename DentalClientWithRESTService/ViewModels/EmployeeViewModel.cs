using DentalClientWithRESTService.HTTPClient;
using DentalClientWithRESTService.Models;
using DentalClientWithRESTService.Views;
using DentalClinicWithRestService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DentalClientWithRESTService.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public List<EmployeeDTO> Employees { get; set; }

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

        public CreateEmployeeDTO CreateEmployee { get; set; } = new CreateEmployeeDTO();

        private PagedResult<EmployeeDTO> _pagedData;
        public PagedResult<EmployeeDTO> PagedData
        {
            get => _pagedData;
            set
            {
                _pagedData = value;
                OnPropertyChanged(nameof(PagedData));
            }
        }

        public EmployeeViewModel()
        {
            Task.Run(async () =>
            {
                var response = await HttpClientProxy.Instance.GetAll("employee");
                _pagedData = await HttpClientProxy.Instance.ReadDataAsList<EmployeeDTO>(response);
            }).Wait();

            Employees = _pagedData.Data.ToList();

            OnPropertyChanged(nameof(Employees));
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
                _pagedData = await HttpClientProxy.Instance.ReadDataAsList<EmployeeDTO>(response);
                Employees = _pagedData.Data.ToList();

                OnPropertyChanged(nameof(Employees));
            });

        private RelayCommand _addEmployee;
        public RelayCommand AddEmployee =>
            _addEmployee ??= new RelayCommand(async (e) =>
            {
                var response = await HttpClientProxy.Instance.Add("employee", CreateEmployee);

                HttpResponse = new HttpResponseWrapper(response);

                var list = await HttpClientProxy.Instance.GetAll("employee");
                _pagedData = await HttpClientProxy.Instance.ReadDataAsList<EmployeeDTO>(response);
                Employees = _pagedData.Data.ToList();

                OnPropertyChanged(nameof(Employees));
            });

        private RelayCommand _nextPage;
        public RelayCommand NextPage =>
            _nextPage ??= new RelayCommand(async (e) =>
            {
                if (_pagedData.PageNumber + 1 > _pagedData.TotalPages) return;
                var response = await HttpClientProxy.Instance.GetAll("employee", pageNumber: _pagedData.PageNumber + 1);
                _pagedData = await HttpClientProxy.Instance.ReadDataAsList<EmployeeDTO>(response);

                Employees = _pagedData.Data.ToList();

                OnPropertyChanged(nameof(Employees));
                OnPropertyChanged(nameof(PagedData));
            });

        private RelayCommand _prevPage;
        public RelayCommand PrevPage =>
            _prevPage ??= new RelayCommand(async (e) =>
            {
                if (_pagedData.PageNumber - 1 < 1) return;
                var response = await HttpClientProxy.Instance.GetAll("employee", pageNumber: _pagedData.PageNumber - 1);
                _pagedData = await HttpClientProxy.Instance.ReadDataAsList<EmployeeDTO>(response);

                Employees = _pagedData.Data.ToList();

                OnPropertyChanged(nameof(Employees));
                OnPropertyChanged(nameof(PagedData));
            });

        #endregion

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
