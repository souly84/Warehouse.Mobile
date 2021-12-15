using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Navigation;
using Warehouse.Mobile.ViewModels;

namespace Warehouse.Mobile
{
    public class PutAwayViewModel : BindableBase, IInitialize
    {
        public PutAwayViewModel()
        {

        }

        private ObservableCollection<LocationViewModel> _reserveLocations;
        public ObservableCollection<LocationViewModel> ReserveLocations
        {
            get => _reserveLocations;
            set => SetProperty(ref _reserveLocations, value);
        }

        private ObservableCollection<LocationViewModel> _raceLocations;
        public ObservableCollection<LocationViewModel> RaceLocations
        {
            get => _raceLocations;
            set => SetProperty(ref _raceLocations, value);
        }


        public void Initialize(INavigationParameters parameters)
        {
            ReserveLocations = new ObservableCollection<LocationViewModel>
            {
                new LocationViewModel
                {
                    Location = "41-1-3", LocationaType = LocationType.Reserve
                },
                new LocationViewModel
                {
                    Location = "42-1-2", LocationaType = LocationType.Reserve
                }
            };

            RaceLocations = new ObservableCollection<LocationViewModel>
            {
                new LocationViewModel
                {
                    Location = "41-1-1", LocationaType = LocationType.Race
                },
                new LocationViewModel
                {
                    Location = "42-1-1", LocationaType = LocationType.Race
                }
            };
        }
    }
}
