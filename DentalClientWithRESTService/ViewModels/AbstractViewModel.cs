using DentalClientWithRESTService.HTTPClient;
using DentalClientWithRESTService.Models;
using DentalClinicWithRestService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DentalClientWithRESTService.ViewModels
{
    public abstract class AbstractViewModel<T> : INotifyPropertyChanged
    {
        private Dictionary<Type, string> _typeToHttpMapper = new Dictionary<Type, string>()
        {
            { typeof(Clinic), "clinic" },
            { typeof(EmployeeDTO), "employee" }
        };

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

        private PagedResult<T> _pagedData;
        public PagedResult<T> PagedData
        {
            get => _pagedData;
            set
            {
                _pagedData = value;
                OnPropertyChanged();
            }
        }

        public List<T> Entities { get; set; }

        private RelayCommand _nextPage;
        public RelayCommand NextPage =>
            _nextPage ??= new RelayCommand(async (e) =>
            {
                if (PagedData.PageNumber + 1 > PagedData.TotalPages) return;
                var response = await HttpClientProxy.Instance.GetAll(_typeToHttpMapper[typeof(T)], pageNumber: PagedData.PageNumber + 1);
                PagedData = await HttpClientProxy.Instance.ReadDataAsList<T>(response);

                Entities = PagedData.Data.ToList();

                OnPropertyChanged(nameof(Entities));
                OnPropertyChanged(nameof(PagedData));
            });

        private RelayCommand _prevPage;
        public RelayCommand PrevPage =>
            _prevPage ??= new RelayCommand(async (e) =>
            {
                if (PagedData.PageNumber - 1 < 1) return;
                var response = await HttpClientProxy.Instance.GetAll(_typeToHttpMapper[typeof(T)], pageNumber: PagedData.PageNumber - 1);
                PagedData = await HttpClientProxy.Instance.ReadDataAsList<T>(response);

                Entities = PagedData.Data.ToList();

                OnPropertyChanged(nameof(Entities));
                OnPropertyChanged(nameof(PagedData));
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
