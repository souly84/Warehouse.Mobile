using System;
using Prism.Mvvm;
using Xamarin.Forms;

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionDateViewModel : BindableBase
    {
        private readonly DateTime _date;

        public ReceptionDateViewModel(DateTime date)
        {
            _date = date;
        }

        private string _dateOfMonth;
        public string DateOfMonth
        {
            get => _dateOfMonth;
            set => SetProperty(ref _dateOfMonth, value);
        }

        private string _dayOfWeek;
        public string DayOfWeek
        {
            get => _dateOfMonth;
            set => SetProperty(ref _dayOfWeek, value);
        }

        private Color _textColor;
        public Color TextColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value);
        }

        private Color _backColor;
        public Color _BackColor
        {
            get => _backColor;
            set => SetProperty(ref _backColor, value);
        }

        public void SelectDate()
        {
        }
    }
}
