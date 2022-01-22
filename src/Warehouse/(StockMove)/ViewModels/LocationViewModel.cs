using System;
using MediaPrint;
using Prism.Mvvm;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class LocationViewModel : BindableBase
    {
        private readonly IStorage _storage;
        private DictionaryMedia? _storageData;

        public LocationViewModel(IStorage storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public int Quantity { get => StorageData().ValueOrDefault<int>("Quantity"); }

        public string Location { get => StorageData().ValueOrDefault<string>("Location"); }

        public IStorage ToStorage()
        {
            return _storage;
        }

        private DictionaryMedia StorageData()
        {
            if (_storageData == null)
            {
                _storageData = _storage.ToDictionary();
            }

            return _storageData;
        }
    }
}
