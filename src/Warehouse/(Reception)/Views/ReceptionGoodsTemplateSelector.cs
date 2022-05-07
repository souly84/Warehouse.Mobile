using Warehouse.Mobile.ViewModels;
using Xamarin.Forms;

namespace Warehouse.Mobile.Views
{
    public class ReceptionGoodsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? Unknown { get; set; }
        public DataTemplate? ExtraConfirmed { get; set; }
        public DataTemplate? Regular { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var good = (ReceptionGoodViewModel)item;
            if (good.IsUnkownGood)
            {
                return Unknown;
            }
            if (good.IsExtraConfirmedReceptionGood)
            {
                return ExtraConfirmed;
            }
            return Regular;
        }
    }
}
