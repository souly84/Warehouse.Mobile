using Prism.Services.Dialogs;
using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.ViewModels
{
    public class LoginViewModel : ScannerViewModel
    {
        public LoginViewModel(IScanner scanner, IDialogService dialog)
            : base(scanner, dialog)
        {
        }

        protected override void OnScan(object sender, IScanningResult barcode)
        {
            base.OnScan(sender, barcode);
        }
    }
}
