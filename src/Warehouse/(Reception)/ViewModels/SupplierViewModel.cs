using System;
using System.Collections.Generic;
using MediaPrint;
using Prism.Commands;
using Prism.Navigation;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class SupplierViewModel
    {
        private readonly ISupplier _supplier;

        public SupplierViewModel(
            ISupplier supplier)
        {
            _supplier = supplier;
        }

        public string Name { get => ((IPrintable)_supplier).ToDictionary().ValueOrDefault<string>("Name"); }

        

    }

    
}
