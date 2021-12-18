using System;
using Prism.Mvvm;

namespace Warehouse.Mobile
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
