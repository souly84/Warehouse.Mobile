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
            _receptionGood = receptionGood ?? throw new ArgumentNullException(nameof(receptionGood));
        }

        public ISupplier Name => _receptionGood.ToDictionary().ValueOrDefault<ISupplier>("Supplier");
    }
}
