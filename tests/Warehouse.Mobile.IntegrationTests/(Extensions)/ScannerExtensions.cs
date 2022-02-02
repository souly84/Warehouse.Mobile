using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.IntegrationTests
{
    public static class ScannerExtensions
    {
        public static void Scan(this IScanner scanner, IScanningResult scanningResult)
        {
            if (scanner is MockScanner mockScanner)
            {
                mockScanner.Scan(scanningResult);
            }
        }
    }
}
