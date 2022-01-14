using System.Threading.Tasks;
using Warehouse.Mobile.Controls;
using Xamarin.Forms;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    public class ReactiveSearchBarTests
    {
        [Fact]
        public async Task KeyPressed_OnTextChange_AfterKeyPressedDelay()
        {
            var keyPressedCount = 0;
            var searchbar = new ReactiveSearchBar();
            searchbar.KeyPressedCommand = new Command(() =>
            {
                keyPressedCount++;
            });
            searchbar.Text = "Hello";
            await Task.Delay(ReactiveSearchBar.TextChangedKeyPressedDelayInMilliseconds * 2);
            Assert.Equal(1, keyPressedCount);
        }

        [Fact]
        public async Task KeyPressedOnlyOnce_OnTextChange_AfterKeyPressedDelay()
        {
            var keyPressedCount = 0;
            var searchbar = new ReactiveSearchBar();
            searchbar.KeyPressedCommand = new Command(() =>
            {
                keyPressedCount++;
            });
            searchbar.Text = "Hello";
            searchbar.Text = "Hello1";
            searchbar.Text = "Hello2";
            await Task.Delay(ReactiveSearchBar.TextChangedKeyPressedDelayInMilliseconds * 2);
            Assert.Equal(1, keyPressedCount);
        }
    }
}
