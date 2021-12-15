using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace Warehouse.Mobile.ViewModels.DatePicker
{
    public class SupportedDateViewModel : BindableBase
    {
        public SupportedDateViewModel()
        {
            AllowedDates = new ObservableCollection<ReceptionDateViewModel>
            {
                new ReceptionDateViewModel(DateTime.Now),
                new ReceptionDateViewModel(DateTime.Now),
                new ReceptionDateViewModel(DateTime.Now),
                new ReceptionDateViewModel(DateTime.Now),
                new ReceptionDateViewModel(DateTime.Now),
                new ReceptionDateViewModel(DateTime.Now),
                new ReceptionDateViewModel(DateTime.Now),
            };
        }

        private ObservableCollection<ReceptionDateViewModel> _allowedDates;
        public ObservableCollection<ReceptionDateViewModel> AllowedDates
        {
            get => _allowedDates;
            set => SetProperty(ref _allowedDates, value);
        }
    }
}
