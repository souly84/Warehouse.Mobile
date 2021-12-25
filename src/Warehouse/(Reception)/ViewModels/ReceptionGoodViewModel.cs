﻿using System;
using System.Windows.Input;
using MediaPrint;
using Prism.Commands;
using Prism.Mvvm;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionGoodViewModel : BindableBase
    {
        private readonly IReceptionGood _receptionGood;
        public bool IsMockedReceptionGood { get => _receptionGood is MockReceptionGood; }

        private DictionaryMedia _goodData;
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

        public ReceptionGoodViewModel(IReceptionGood receptionGood)
        {
            _receptionGood = receptionGood;
        }

        

        public int Total { get; set; }

        public int ConfirmedQuantity
        {
            get => _receptionGood.Confirmation.ToDictionary().Value<int>("Confirmed");
        }

        public string Name { get => GoodData.ValueOrDefault<string>("Article"); }

        public int Quantity  { get => GoodData.ValueOrDefault<int>("Quantity"); }

        public string Oa { get => GoodData.ValueOrDefault<string>("oa"); }


        private DelegateCommand increaseQuantityCommand;

        public DelegateCommand IncreaseQuantityCommand => increaseQuantityCommand ?? (increaseQuantityCommand = new DelegateCommand(async () =>
        {
            _receptionGood.Confirmation.Increase(1);
            RaisePropertyChanged(nameof(ConfirmedQuantity));
        }));

        private DelegateCommand decreaseQuantityCommand;

        public DelegateCommand DecreaseQuantityCommand => decreaseQuantityCommand ?? (decreaseQuantityCommand = new DelegateCommand(async () =>
        {
             _receptionGood.Confirmation.Decrease(1);
            RaisePropertyChanged(nameof(ConfirmedQuantity));
        }));

        public override bool Equals(object obj)
        {
            return object.ReferenceEquals(this, obj)
                || obj is IReceptionGood receptionGood
                && _receptionGood.Equals(receptionGood);
        }

        



    }
}
