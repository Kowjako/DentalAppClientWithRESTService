using DentalClientWithRESTService.HTTPClient;
using DentalClientWithRESTService.Models;
using DentalClinicWithRestService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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

        public SearchModel SearchModel { get; set; } = new SearchModel();

        public async Task RefreshList()
        {
            var pagedResultResponse = await HttpClientProxy.Instance.GetAll(_typeToHttpMapper[typeof(T)]);
            PagedData = await HttpClientProxy.Instance.ReadDataAsList<T>(pagedResultResponse);
            Entities = PagedData.Data.ToList();

            OnPropertyChanged(nameof(Entities));
            OnPropertyChanged(nameof(PagedData));
        }

        private RelayCommand _nextPage;
        public RelayCommand NextPage =>
            _nextPage ??= new RelayCommand(async (e) =>
            {
                if (PagedData.PageNumber + 1 > PagedData.TotalPages) return;
                var response = await HttpClientProxy.Instance.GetAll(_typeToHttpMapper[typeof(T)],
                                                                     searchPhrase: SearchModel.SearchPhrase,
                                                                     sortDirection: SearchModel.SortDirectionNormalized,
                                                                     sortBy: SearchModel.SortByNormalized,
                                                                     pageNumber: PagedData.PageNumber + 1);
                HttpResponse = new HttpResponseWrapper(response);
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
                var response = await HttpClientProxy.Instance.GetAll(_typeToHttpMapper[typeof(T)],
                                                                     searchPhrase: SearchModel.SearchPhrase,
                                                                     sortDirection: SearchModel.SortDirectionNormalized,
                                                                     sortBy: SearchModel.SortByNormalized,
                                                                     pageNumber: PagedData.PageNumber - 1);
                HttpResponse = new HttpResponseWrapper(response);
                PagedData = await HttpClientProxy.Instance.ReadDataAsList<T>(response);

                Entities = PagedData.Data.ToList();

                OnPropertyChanged(nameof(Entities));
                OnPropertyChanged(nameof(PagedData));
            });

        private RelayCommand _advancedSearch;
        public RelayCommand AdvancedSearch =>
            _advancedSearch ??= new RelayCommand(async (e) =>
            {
                var response = await HttpClientProxy.Instance.GetAll(_typeToHttpMapper[typeof(T)], 
                                                                     searchPhrase: SearchModel.SearchPhrase,
                                                                     sortDirection: SearchModel.SortDirectionNormalized,
                                                                     sortBy: SearchModel.SortByNormalized);
                PagedData = await HttpClientProxy.Instance.ReadDataAsList<T>(response);
                HttpResponse = new HttpResponseWrapper(response);

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
