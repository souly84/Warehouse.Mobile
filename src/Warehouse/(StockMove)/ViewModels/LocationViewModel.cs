using System;
using System.Threading.Tasks;
using MediaPrint;
using Prism.Mvvm;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Mobile.ViewModels
{
    public class LocationViewModel : BindableBase
    {
        private readonly IStorage _storage;
        private DictionaryMedia? _storageData;
        private AsyncData<int>? _quantity;

        public LocationViewModel(IStorage storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public AsyncData<int> Quantity
        {
            get
            {
                if (_quantity == null)
                {
                    _quantity = new AsyncData<int>(_storage.TotalGoodsQuantityAsync());
                }
                return _quantity;
            }
        }

        public string Location { get => StorageData().ValueOrDefault<string>("Location", "Ean") ?? string.Empty; }

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
