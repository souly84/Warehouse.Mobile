using System;
using System.IO;
using System.Net;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Views.InputMethods;
using Plugin.CurrentActivity;
using Warehouse.Mobile.Interfaces;
using Xamarin.Forms;

namespace Warehouse.Droid.Environment
{
    public class AndroidEnvironment : IEnvironment
    {
        public string GetBuildVersion()
        {
            var context = Android.App.Application.Context;
            if (context == null)
            {
                return "";
            }

            var buildVersion = context.ApplicationContext.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            return buildVersion;
        }

        public void ExitApp()
        {
            var context = CrossCurrentActivity.Current.Activity;
            if (context == null)
            {
                return;
            }

            context.FinishAffinity();
            Process.KillProcess(Process.MyPid());
        }

        public string GetApplicationPath()
        {
            return Android.OS.Environment.ExternalStorageDirectory.Path;
        }

        public string GetCurrentFirstIPAddress()
        {
            var adresses = Dns.GetHostAddresses(Dns.GetHostName());

            return adresses?[0]?.ToString() ?? "";
        }

        public string LoadFile(string fileName)
        {
            var documentsPath = GetApplicationPath();
            var filePath = Path.Combine(documentsPath, fileName);
            return File.ReadAllText(filePath);
        }

        public void SaveFile(string fileName, string text)
        {
            var documentsPath = GetApplicationPath();
            var filePath = Path.Combine(documentsPath, fileName);
            File.WriteAllText(filePath, text);
        }

        public string GetDeviceId()
        {
            try
            {
                return Build.Serial;
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error(nameof(AndroidEnvironment), ex.Message);
                return "";
            }
        }

        public void HideKeyboard()
        {
            var currentActivity = CrossCurrentActivity.Current.Activity;
            if (currentActivity == null)
            {
                return;
            }

            var inputManager = (InputMethodManager)currentActivity.GetSystemService(Context.InputMethodService);
            var currentFocus = currentActivity.CurrentFocus;

            if (currentFocus == null)
            {
                return;
            }

            inputManager?.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
            currentActivity.CurrentFocus.ClearFocus();
        }

        public void BeepLoudly()
        {
            new ToneGenerator(Android.Media.Stream.Notification, 100).StartTone(Tone.CdmaSoftErrorLite, 150);
        }
    }
}
