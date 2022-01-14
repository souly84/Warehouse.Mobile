using System;
using System.Threading.Tasks;
using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.UnitTests.Mocks
{
    public class FailedBarcodeScanner : ITestScanner
    {
        private readonly Exception _ex;
        private readonly bool _onScanOnly;

        public FailedBarcodeScanner(Exception ex, bool onScanOnly = false)
        {
            _ex = ex;
            _onScanOnly = onScanOnly;
        }

        public ScannerState State => _onScanOnly ? ScannerState.Closed : throw _ex;

        public event EventHandler<IScanningResult> OnScan;

        public void BeepFailure()
        {
            if (!_onScanOnly)
            {
                throw _ex;
            }
        }

        public void BeepSuccess()
        {
            if (!_onScanOnly)
            {
                throw _ex;
            }
        }

        public Task CloseAsync()
        {
            if (!_onScanOnly)
            {
                throw _ex;
            }
            return Task.CompletedTask;
        }

        public Task EnableAsync(bool enabled)
        {
            if (!_onScanOnly)
            {
                throw _ex;
            }
            return Task.CompletedTask;
        }

        public Task OpenAsync()
        {
            if (!_onScanOnly)
            {
                throw _ex;
            }
            return Task.CompletedTask;
        }

        public void Scan(IScanningResult result)
        {
            throw _ex;
        }
    }
}
