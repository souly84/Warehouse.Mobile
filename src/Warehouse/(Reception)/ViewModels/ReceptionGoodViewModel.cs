using System;
using MediaPrint;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionGoodViewModel
    {
        private readonly IReceptionGood _receptionGood;

        public ReceptionGoodViewModel(IReceptionGood receptionGood)
        {
            _receptionGood = receptionGood;
        }

        //public string Name { get => ((IPrintable)_receptionGood).ToDictionary().ValueOrDefault<string>("Name"); }
        public string Name { get => "Fake description"; }

        //public int Quantity { get => ((IPrintable)_receptionGood).ToDictionary().ValueOrDefault<int>("Quantity"); }
        public int Quantity  { get => 2; }

        //public string OA { get => ((IPrintable)_receptionGood).ToDictionary().ValueOrDefault<string>("Oa"); }
        public string Oa { get => "OA298756"; }

    }
}
