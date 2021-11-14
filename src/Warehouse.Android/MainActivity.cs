using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Prism.Navigation;
using Resource = Xamarin.Forms.Platform.Android.Resource;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Warehouse.Mobile.Droid
{
    [Activity(Label = "Warehouse", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        public INavigationService Navigation => ((App)App.Current).Navigation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            var application = new App(new AndroidInitializer(this));

            LoadApplication(application);

            AppCenter.Start("dbafbbc1-b7fc-4dec-b29e-36015da6bc4e",
                   typeof(Analytics), typeof(Crashes));
        }
    }
}