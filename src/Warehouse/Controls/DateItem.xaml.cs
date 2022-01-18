using System;
using Xamarin.Forms;

namespace Warehouse.Mobile.Controls
{
    public partial class DateItem 
    {
        public static readonly BindableProperty TextColorProperty =
           BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(DateItem),
               propertyChanged: OnTextColorChanged);

        public static readonly BindableProperty DateProperty =
           BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(DateItem),
               propertyChanged: OnDateChanged);

        public DateItem()
        {
            InitializeComponent();
        }

        public Color TextColor
        {
            set => SetValue(TextColorProperty, value);
            get => (Color)GetValue(TextColorProperty);
        }

        public DateTime Date
        {
            set => SetValue(DateProperty, value);
            get => (DateTime)GetValue(DateProperty);
        }

        private static void OnTextColorChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((DateItem)bindable).DayLabel.TextColor = (Color)newvalue;
            ((DateItem)bindable).DateLabel.TextColor = (Color)newvalue;
        }

        private static void OnDateChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((DateItem)bindable).DayLabel.Text = ((DateTime)newvalue).Day.ToString();
            ((DateItem)bindable).DateLabel.Text = ((DateTime)newvalue).DayOfWeek.ToString();
        }
    }
}
