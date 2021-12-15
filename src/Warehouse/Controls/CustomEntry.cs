using System;
using System.Windows.Input;
using Xamarin.Forms;
namespace Warehouse.Mobile.Controls
{
    public class CustomEntry : Entry
    {
        public static readonly BindableProperty DigitsTypesToAllowProperty = BindableProperty.Create(
            nameof(DigitsTypesToAllow),
            typeof(AllowedDigitTypes),
            typeof(CustomEntry),
            AllowedDigitTypes.Everything);

        public static readonly BindableProperty NumberOfDecimalAcceptedProperty = BindableProperty.Create(
            nameof(NumberOfDecimalAccepted),
            typeof(int),
            typeof(CustomEntry),
            2);

        public static readonly BindableProperty EnterCommandProperty = BindableProperty.Create(
            nameof(EnterCommand),
            typeof(ICommand),
            typeof(CustomEntry),
            null);

        public static readonly BindableProperty KeyPressedCommandProperty = BindableProperty.Create(
            nameof(KeyPressedCommand),
            typeof(ICommand),
            typeof(CustomEntry),
            null);

        // Used in Windows Phone only
        public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(
            nameof(MaxValue),
            typeof(int),
            typeof(CustomEntry),
            int.MaxValue);

        public Action SelectAll { get; set; }

        public virtual AllowedDigitTypes DigitsTypesToAllow
        {
            get { return (AllowedDigitTypes)GetValue(DigitsTypesToAllowProperty); }
            set { SetValue(DigitsTypesToAllowProperty, value); }
        }

        public virtual int NumberOfDecimalAccepted
        {
            get { return (int)GetValue(NumberOfDecimalAcceptedProperty); }
            set { SetValue(NumberOfDecimalAcceptedProperty, value); }
        }

        public ICommand EnterCommand
        {
            get { return (ICommand)GetValue(EnterCommandProperty); }
            set { SetValue(EnterCommandProperty, value); }
        }

        public ICommand KeyPressedCommand
        {
            get { return (ICommand)GetValue(KeyPressedCommandProperty); }
            set { SetValue(KeyPressedCommandProperty, value); }
        }

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
    }
}
