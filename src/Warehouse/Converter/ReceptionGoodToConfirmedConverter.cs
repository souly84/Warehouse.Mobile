using System;
using System.Globalization;
using Warehouse.Mobile.ViewModels;
using Xamarin.Forms;

namespace Warehouse.Mobile.Converter
{
    public class ReceptionGoodToConfirmedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            
            var vm = (ReceptionGoodViewModel)value;
            if (vm.Quantity == 1000)
            {
                return vm.ConfirmedQuantity;
            }
            return $"{vm.ConfirmedQuantity}/{vm.Quantity}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
