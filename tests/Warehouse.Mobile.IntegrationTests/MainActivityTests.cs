using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Warehouse.Mobile.IntegrationTests.AndroidInstrumentations;
using Xunit;

namespace Warehouse.Mobile.IntegrationTests
{
    public class MainActivityTests
    {
        private TestInstrumentation instrument = TestInstrumentation.CurrentInstrumentation;

        [Fact(Skip = "Doesnt work at the moment")]
        public async Task MainActivityCreation()
        {
            Assert.NotNull(await ActivityAsync());
        }

        private async Task<Activity> ActivityAsync()
        {
            if (instrument != null)
            {
                Instrumentation.ActivityMonitor monitor = instrument.AddMonitor("warehouse.mobile.android.mainactivity", null, false);

                Intent intent = new Intent();
                intent.AddFlags(ActivityFlags.NewTask);
                intent.SetClassName(instrument.TargetContext, "warehouse.mobile.android.mainactivity");

                await Task.Run(() => instrument.StartActivitySync(intent));

                return instrument.WaitForMonitor(monitor);
            }
            return null;
        }
    }
}
