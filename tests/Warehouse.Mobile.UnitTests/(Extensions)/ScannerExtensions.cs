using Warehouse.Core.Plugins;
using Warehouse.Mobile.UnitTests.Mocks;

namespace Warehouse.Mobile.UnitTests
{
    public static class ScannerExtensions
    {
        public static void Scan(this IScanner scanner, IScanningResult scanningResult)
        {
            if (scanner is MockScanner mockScanner)
            {
                mockScanner.Scan(scanningResult);
            }
            else
            {
                ((ITestScanner)scanner).Scan(scanningResult);
            }
        }
    }
}
