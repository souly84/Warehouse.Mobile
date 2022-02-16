using System;
using System.Windows.Input;
using Dotnet.Commands;
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
        private readonly CachedCommands _commands;
        private string? _errorMessage;

        public ReceptionGoodViewModel(
            IReceptionGood receptionGood,
            ICommands commands)
        {
            _receptionGood = receptionGood ?? throw new ArgumentNullException(nameof(receptionGood));
            ErrorMessage = IsUnkownGood ? $"Not expected {_receptionGood.ToDictionary().ValueOrDefault<string>("Barcode")}" : "Received more than expected";
            _commands = commands.Cached();
        }

        public bool IsUnkownGood => _receptionGood.IsUnknown;

        public bool IsExtraConfirmedReceptionGood => _receptionGood.IsExtraConfirmed;

        public bool Regular => !IsExtraConfirmedReceptionGood && !IsUnkownGood;

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

        public int ConfirmedQuantity => _receptionGood.Confirmation.ConfirmedQuantity;

        public string Name => GoodData.ValueOrDefault<string>("Article");

        public int Quantity => GoodData.ValueOrDefault<int>("Quantity");

        public string Oa => GoodData.ValueOrDefault<string>("oa");

        public string? ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public string RemainingQuantity
        {
            get 
            {
                if (_receptionGood.IsUnknown)
                {
                    return ConfirmedQuantity.ToString();
                }
                return $"{ConfirmedQuantity}/{Quantity}";
            }
        }
        
        public ICommand IncreaseQuantityCommand => _commands.Command(() =>
        {
            _receptionGood.Confirmation.Increase(1);
            RaisePropertyChanged(nameof(ConfirmedQuantity));
            RaisePropertyChanged(nameof(RemainingQuantity));

        });

        public ICommand DecreaseQuantityCommand => _commands.Command(() =>
        {
            _receptionGood.Confirmation.Decrease(1);
            RaisePropertyChanged(nameof(ConfirmedQuantity));
            RaisePropertyChanged(nameof(RemainingQuantity));
        });

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
