using Warehouse.Mobile.Extensions;
using Xamarin.Forms;

namespace Warehouse.Mobile.Controls
{
    public static class BindableFocus
    {
        public static readonly BindableProperty IsFocusedProperty =
            BindableProperty.CreateAttached("IsFocused", typeof(bool), typeof(BindableFocus), false, BindingMode.TwoWay, propertyChanged: OnIsFocusedChanged);

        public static bool GetIsFocused(BindableObject bindable)
        {
            return (bool)bindable.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(BindableObject bindable, bool value)
        {
            bindable.SetValue(IsFocusedProperty, value);
        }

        public static void OnIsFocusedChanged(object sender, object oldValue, object newvalue)
        {
            OnIsFocusedChanged((VisualElement)sender, (bool)oldValue, (bool)newvalue);
        }

        public static async void OnIsFocusedChanged(VisualElement sender, bool oldValue, bool newValue)
        {
            if (sender == null)
            {
                return;
            }

            if (newValue && sender.IsEnabled)
            {
                await sender.FocusAsync().ConfigureAwait(true);
                var entry = sender as CustomEntry;
                if (entry != null)
                {
                    entry.SelectAll?.Invoke();
                }
            }
            else
            {
                sender.SetValue(IsFocusedProperty, false);
                sender.Unfocus();
            }
        }
    }
}
