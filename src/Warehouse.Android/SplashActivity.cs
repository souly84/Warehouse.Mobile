using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Warehouse.Mobile.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : Activity, Animator.IAnimatorListener
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public void OnAnimationCancel(Animator animation)
        {
        }

        public void OnAnimationEnd(Animator animation)
        {
            
        }

        public void OnAnimationRepeat(Animator animation)
        {
        }

        public void OnAnimationStart(Animator animation)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

          //  SetContentView(Resource.Layout.splash_layout);
            //var animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            //animationView.AddAnimatorListener(this);
            StartActivity(new Intent(this, typeof(MainActivity)));
        }
    }
}