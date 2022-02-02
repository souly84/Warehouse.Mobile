using System;
using MediaPrint;
using Prism.Commands;
using Prism.Mvvm;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionGoodViewModel : BindableBase
    {
        private readonly IReceptionGood _receptionGood;
        private DictionaryMedia? _goodData;

        public ReceptionGoodViewModel(IReceptionGood receptionGood)
        {
            _receptionGood = receptionGood ?? throw new ArgumentNullException(nameof(receptionGood));
        }

        public bool IsUnkownGood => _receptionGood.IsUnknown;

        public bool IsExtraConfirmedReceptionGood => _receptionGood.IsExtraConfirmed;

        private DictionaryMedia GoodData
        {
            get
            {
                if (_goodData == null)
                {
                    _goodData = _receptionGood.ToDictionary();
                }
                return _goodData;
            }
        }

        public int Total { get; set; }

        public int ConfirmedQuantity => _receptionGood.Confirmation.ToDictionary().Value<int>("Confirmed");

        public string Name => GoodData.ValueOrDefault<string>("Article");

        public int Quantity => GoodData.ValueOrDefault<int>("Quantity");

        public string Oa => GoodData.ValueOrDefault<string>("oa");

        private DelegateCommand? increaseQuantityCommand;
        public DelegateCommand IncreaseQuantityCommand => increaseQuantityCommand ?? (increaseQuantityCommand = new DelegateCommand(() =>
        {
            _receptionGood.Confirmation.Increase(1);
            RaisePropertyChanged(nameof(ConfirmedQuantity));
        }));

        private DelegateCommand? decreaseQuantityCommand;
        public DelegateCommand DecreaseQuantityCommand => decreaseQuantityCommand ?? (decreaseQuantityCommand = new DelegateCommand(() =>
        {
            _receptionGood.Confirmation.Decrease(1);
            RaisePropertyChanged(nameof(ConfirmedQuantity));
        }));

        public override bool Equals(object obj)
        {
            return object.ReferenceEquals(this, obj)
                || _receptionGood.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _receptionGood.GetHashCode();
        }
    }
}
