using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Prism.Navigation;
using Resource = Xamarin.Forms.Platform.Android.Resource;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Warehouse.Droid.Permissions;

namespace Warehouse.Mobile.Droid
{
    [Activity(Label = "Warehouse",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Name = "warehouse.mobile.android.mainactivity")]
    public class MainActivity : FormsAppCompatActivity
    {
        private App _app;

        public INavigationService Navigation => ((App)App.Current).Navigation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            _app = new App(new AndroidInitializer(this));
            LoadApplication(_app);

            AppCenter.Start("dbafbbc1-b7fc-4dec-b29e-36015da6bc4e",
                   typeof(Analytics), typeof(Crashes));

            new ManifestPermissions(this).Request();
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await _app.Scanner.OpenAsync();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _app.Scanner.CloseAsync().ConfigureAwait(false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _app.Scanner.CloseAsync().ConfigureAwait(false);
        }
    }
}