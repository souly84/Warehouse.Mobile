using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Prism.Navigation;
using Resource = Xamarin.Forms.Platform.Android.Resource;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Warehouse.Core.Plugins;
using Prism.Plugin.Popups;

namespace Warehouse.Mobile.Droid
{
    [Activity(Label = "Warehouse", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        public INavigationService Navigation => ((App)App.Current).Navigation;
        private App application;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Rg.Plugins.Popup.Popup.Init(this);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);

            application = new App(new AndroidInitializer(this));

            LoadApplication(application);

            AppCenter.Start("dbafbbc1-b7fc-4dec-b29e-36015da6bc4e",
                   typeof(Analytics), typeof(Crashes));
        }

        protected override void OnResume()
        {
            base.OnResume();
            _ = application.Scanner.OpenAsync();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _ = application.Scanner.CloseAsync();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _ = application.Scanner.CloseAsync();
        }

        public override void OnBackPressed()
        {
            PopupPlugin.OnBackPressed();
        }
    }
}