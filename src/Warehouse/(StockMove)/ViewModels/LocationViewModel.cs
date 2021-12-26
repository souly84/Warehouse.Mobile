using System;
using MediaPrint;
using Prism.Mvvm;
using Warehouse.Core;

namespace Warehouse.Mobile
{
    public class LocationViewModel : BindableBase
    {
        private readonly IStorage _storage;

        public LocationViewModel(IStorage storage)
        {
            _storage = storage;
        }

        public int Quantity { get => _storage.ToDictionary().ValueOrDefault<int>("Quantity"); }

        //public string Location { get => _storage.ToDictionary().ValueOrDefault<string>("Location"); }

        public IStorage ToStorage()
        {
            return _storage;
        }

    }
}
