using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Prism.Navigation;
using Resource = Xamarin.Forms.Platform.Android.Resource;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.Plugin.Popups;
using Warehouse.Droid.Permissions;
using PerpetualEngine.Storage;
using Plugin.CurrentActivity;

namespace Warehouse.Mobile.Droid
{
    [Activity(Label = "Warehouse",
        Icon = "@mipmap/customicon",
        Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait,
        Name = "warehouse.mobile.android.mainactivity")]
    public class MainActivity : FormsAppCompatActivity
    {
        public App App { get; private set; }

        public INavigationService Navigation => App.Navigation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            SimpleStorage.SetContext(ApplicationContext);
            global::Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            App = new App(new AndroidInitializer(this));
            LoadApplication(App);

            AppCenter.Start("dbafbbc1-b7fc-4dec-b29e-36015da6bc4e",
                   typeof(Analytics), typeof(Crashes));

            new ManifestPermissions(this).Request();
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await App.Scanner.OpenAsync();
        }

        protected override void OnPause()
        {
            base.OnPause();
            App.Scanner.CloseAsync().ConfigureAwait(false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            App.Scanner.CloseAsync().ConfigureAwait(false);
        }

        public override void OnBackPressed()
        {
            PopupPlugin.OnBackPressed();
        }
    }
}