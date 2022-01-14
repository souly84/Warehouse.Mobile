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
            var keyPressed = false;
            var searchbar = new ReactiveSearchBar();
            searchbar.KeyPressedCommand = new Command(() =>
            {
                keyPressed = true;
            });
            searchbar.Text = "Hello";
            await Task.Delay(ReactiveSearchBar.TextChangedKeyPressedDelayInMilliseconds * 2);
            Assert.True(keyPressed);
        }
    }
}
