using System;
using MediaPrint;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionViewModel
    {
        private readonly IReceptionGood _receptionGood;

        public ReceptionViewModel(IReceptionGood receptionGood)
        {
            _receptionGood = receptionGood;
        }

        public ISupplier Name { get => ((IPrintable)_receptionGood).ToDictionary().ValueOrDefault<ISupplier>("Supplier"); }

    }
}
