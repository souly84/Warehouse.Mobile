using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Warehouse.Mobile.IntegrationTests.AndroidInstrumentations;

namespace Warehouse.Mobile.IntegrationTests
{
    public class InstrumentationActivity<T> : IDisposable
        where T : Activity
    {
        private readonly TestInstrumentation _instrumentation;
        private Instrumentation.ActivityMonitor _monitor;
        private List<Activity> _activities = new List<Activity>();

        public InstrumentationActivity(TestInstrumentation instrumentation)
        {
            _instrumentation = instrumentation;
        }

        public async Task<T> ActivityAsync()
        {
            var activityName = new ActivityName(typeof(T)).ToString();
            _monitor = _instrumentation.AddMonitor(
                activityName, null, false
            );

            Intent intent = new Intent();
            intent.AddFlags(ActivityFlags.NewTask);
            intent.SetClassName(_instrumentation.TargetContext, activityName);
            await Task.Run(() => _instrumentation.StartActivitySync(intent)).WithTimeout(10000);
            var activity = (T)_instrumentation.WaitForMonitorWithTimeout(_monitor, 10000);
            _activities.Add(activity);
            return activity;
        }

        public void Dispose()
        {
            if (_monitor != null)
            {
                foreach (var activity in _activities)
                {
                    activity.Finish();
                }
               
                _instrumentation.RemoveMonitor(_monitor);
            }
        }
    }
}
