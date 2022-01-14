using System;
using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.UnitTests.Mocks
{
    public class ErrorScanningResult : IScanningResult
    {
        private readonly Exception _ex;

        public ErrorScanningResult(string errorMessage)
            : this(new Exception(errorMessage))
        {
        }

        public ErrorScanningResult(Exception ex)
        {
            _ex = ex;
        }

        public string BarcodeData => throw _ex;

        public string Symbology => throw _ex;

        public TimeSpan Timestamp => throw _ex;
    }
}
