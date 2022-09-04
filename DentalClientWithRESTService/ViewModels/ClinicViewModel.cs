﻿using DentalClientWithRESTService.HTTPClient;
using DentalClientWithRESTService.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DentalClientWithRESTService.ViewModels
{
    public class ClinicViewModel : INotifyPropertyChanged
    {
        public List<Clinic> Clinics { get; set; }

        public ClinicViewModel()
        {
            Task.Run(async () => Clinics = await HttpClientProxy.Instance.GetAllClinic()).Wait();
            OnPropertyChanged(nameof(Clinics));
        }



        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
