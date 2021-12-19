using System.Linq;
using Android.App;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace Warehouse.Droid.Permissions
{
    public class ManifestPermissions : IManifestPermissions
    {
        private const int PermissionRequestId = 1;
        private readonly Activity _activity;

        public ManifestPermissions(Activity activity)
        {
            _activity = activity;
        }

        public void Request()
        {
            var manifestPermissions = _activity.PackageManager
                .GetPackageInfo(_activity.PackageName, PackageInfoFlags.Permissions)
                .RequestedPermissions;

            ActivityCompat.RequestPermissions(_activity,
                manifestPermissions.Where(permission =>
                    ContextCompat.CheckSelfPermission(_activity, permission) != Permission.Granted).ToArray(),
                PermissionRequestId);
        }
    }
}
