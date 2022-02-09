using System.Threading.Tasks;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.IntegrationTests.AndroidInstrumentations;
using Warehouse.Mobile.ViewModels;
using Warehouse.Scanner.SDK;
using Xunit;

namespace Warehouse.Mobile.IntegrationTests
{
    public class MainActivityTests
    {
        private TestInstrumentation instrument = TestInstrumentation.CurrentInstrumentation;

        [Fact]
        public async Task ScannerOpened()
        {
            if (instrument != null)
            {
                using (var activity = new InstrumentationActivity<Droid.MainActivity>(instrument))
                {
                    var mainActivity = await activity.ActivityAsync();
                    Assert.NotNull(
                        mainActivity.App.Scanner
                    );

                    await mainActivity.App.Scanner
                        .WaitIfTransitionAsync(ScannerState.Closed)
                        .WithTimeout(30000);
                    Assert.Equal(
                        ScannerState.Opened,
                        mainActivity.App.Scanner.State
                    );
                }
            }
        }

        [Fact]
        public async Task NavigationCreated()
        {
            if (instrument != null)
            {
                using (var activity = new InstrumentationActivity<Droid.MainActivity>(instrument))
                {
                    Assert.NotNull(
                       (await activity.ActivityAsync()).Navigation
                    );
                }
            }
        }

        [Fact]
        public async Task MenuSelectionAsDefaultPage()
        {
            if (instrument != null)
            {
                using (var activity = new InstrumentationActivity<Droid.MainActivity>(instrument))
                {
                    var mainActivity = await activity.ActivityAsync();
                    Assert.IsType<MenuSelectionViewModel>(
                        mainActivity.App.CurrentViewModel<object>()
                    );
                }
            }
        }
    }
}
