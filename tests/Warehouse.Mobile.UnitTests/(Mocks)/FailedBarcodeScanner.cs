using System;
using System.Threading.Tasks;
using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.UnitTests.Mocks
{
    public class FailedBarcodeScanner : IScanner
    {
        private readonly Exception _ex;

        public FailedBarcodeScanner(Exception ex)
        {
            _ex = ex;
        }

        public ScannerState State => throw _ex;

        public event EventHandler<IScanningResult> OnScan;

        public void BeepFailure()
        {
            throw _ex;
        }

        public void BeepSuccess()
        {
            throw _ex;
        }

        public Task CloseAsync()
        {
            throw _ex;
        }

        public Task EnableAsync(bool enabled)
        {
            throw _ex;
        }

        public Task OpenAsync()
        {
            throw _ex;
        }
    }
}
