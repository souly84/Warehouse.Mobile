using System;
using System.Linq;
using Android.App;

namespace Warehouse.Mobile.IntegrationTests
{
    public class ActivityName
    {
        private readonly Type _activityType;

        public ActivityName(Type activityType)
        {
            _activityType = activityType;
        }

        public override string ToString()
        {
            var activityAttribute = (ActivityAttribute)_activityType
                .GetCustomAttributes(true)
                .Where(a => a is ActivityAttribute)
                .First();
            return activityAttribute.Name;
        }
    }
}
