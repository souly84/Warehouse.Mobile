using System;
using System.Threading.Tasks;
using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.UnitTests.Mocks
{
    public class FailedScanner : IScanner
    {
        private readonly Exception _exception;

        public FailedScanner(Exception exception)
        {
            _exception = exception;
        }

        public ScannerState State => throw _exception;

        public event EventHandler<IScanningResult> OnScan;

        public void BeepFailure()
        {
            throw _exception;
        }

        public void BeepSuccess()
        {
            throw _exception;
        }

        public Task CloseAsync()
        {
            throw _exception;
        }

        public Task EnableAsync(bool enabled)
        {
            throw _exception;
        }

        public Task OpenAsync()
        {
            throw _exception;
        }
    }
}
