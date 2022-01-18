using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.UnitTests.Mocks
{
    public interface ITestScanner : IScanner
    {
        void Scan(IScanningResult result);
    }
}
