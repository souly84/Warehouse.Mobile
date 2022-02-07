using System;
namespace Warehouse.Mobile.Helper
{
    public interface IDeviceHelper
    {
        string GetBuildVersion();
    }

    public class MockDeviceHelper : IDeviceHelper
    {
        public string GetBuildVersion()
        {
            return "T001";
        }
    }
}
