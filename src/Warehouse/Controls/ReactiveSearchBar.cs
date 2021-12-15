using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Warehouse.Mobile.Controls
{
    public class ReactiveSearchBar : SearchBar
    {
        private CancellationTokenSource? cts;

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
            cts?.Cancel();
            cts = new CancellationTokenSource();
            Task.Delay(300, cts.Token)
                .ContinueWith(
                    _ =>
                    {
                        if (KeyPressedCommand != null && KeyPressedCommand.CanExecute(null))
                        {
                            KeyPressedCommand.Execute(null);
                        }
                    },
                    cts.Token)
                .ConfigureAwait(false);
        }
    }
}
