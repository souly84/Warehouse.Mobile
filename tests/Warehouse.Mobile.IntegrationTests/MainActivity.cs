using Android.App;
using Android.OS;
using Xunit.Runners.UI;
using Xunit.Sdk;

namespace Warehouse.Mobile.IntegrationTests
{
    [Activity(Label = "Warehouse.Mobile.IntegrationTests", MainLauncher = true)]
    public class MainActivity : RunnerActivity
    {
        public static MainActivity Current;

        protected override void OnCreate(Bundle bundle)
        {
            Current = this;

            AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);

            // tests can be inside the main assembly
            AddTestAssembly(typeof(MainActivity).Assembly);

            // Once you called base.OnCreate(), you cannot add more assemblies.
            base.OnCreate(bundle);
        }
    }
}
