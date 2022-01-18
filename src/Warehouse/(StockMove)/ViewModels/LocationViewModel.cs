using System;
using Prism.Mvvm;

namespace Warehouse.Mobile.ViewModels
{
    public class LocationViewModel : BindableBase
    {
        public string Location { get; set; }
        public LocationType LocationaType { get; set; }

        public LocationViewModel()
        {
        }
    }
}
