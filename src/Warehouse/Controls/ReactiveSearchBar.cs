using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Warehouse.Mobile.Controls
{
    public class ReactiveSearchBar : SearchBar
    {
        public const int TextChangedKeyPressedDelayInMilliseconds = 300;

        private CancellationTokenSource? _cts;

        public static readonly BindableProperty KeyPressedCommandProperty = BindableProperty.Create(
            nameof(KeyPressedCommand),
            typeof(ICommand),
            typeof(CustomEntry),
            null);

        public ReactiveSearchBar()
        {
            TextChanged += OnTextChanged;
        }

        public ICommand KeyPressedCommand
        {
            get { return (ICommand)GetValue(KeyPressedCommandProperty); }
            set { SetValue(KeyPressedCommandProperty, value); }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            Task.Delay(TextChangedKeyPressedDelayInMilliseconds, _cts.Token)
                .ContinueWith(
                    _ =>
                    {
                        if (KeyPressedCommand != null && KeyPressedCommand.CanExecute(null))
                        {
                            KeyPressedCommand.Execute(null);
                        }
                    },
                    _cts.Token)
                .ConfigureAwait(false);
        }
    }
}
