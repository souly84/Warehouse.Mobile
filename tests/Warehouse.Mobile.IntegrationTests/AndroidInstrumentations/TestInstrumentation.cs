using System;
using System.Reflection;

using Android.App;
using Android.Runtime;
using Xunit.Runners.TestSuiteInstrumentation;

namespace Warehouse.Mobile.IntegrationTests.AndroidInstrumentations
{
    [Instrumentation(Name = "warehouse.mobile.testinstrumentation")]
    public class TestInstrumentation : XunitTestSuiteInstrumentation
    {
        public TestInstrumentation(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        protected override void AddTests()
        {
            AddTestAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
