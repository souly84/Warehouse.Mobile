using System;
using Android.App;
using Microsoft.AppCenter.Crashes;
using Warehouse.Mobile.Helper;

namespace Warehouse.Droid.Services
{
    public class DeviceHelper : IDeviceHelper
    {
        public string GetBuildVersion()
        {
            return Application.Context?.ApplicationContext?.PackageManager?.GetPackageInfo(Application.Context.PackageName, 0).VersionName;
        }
    }
}
