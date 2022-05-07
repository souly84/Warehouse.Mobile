using System;
using System.Threading.Tasks;
using Prism.Services;

namespace Warehouse.Mobile
{
    public static class DialogExtensions
    {
        public static Task ErrorAsync(this IPageDialogService dialog, Exception exception)
        {
            return dialog.ErrorAsync(
                exception.Message
            );
        }

        public static Task ErrorAsync(this IPageDialogService dialog, string errorMessage)
        {
            return dialog.DisplayAlertAsync(
                "Error",
                errorMessage,
                "Ok"
            );
        }

        public static Task<bool> WarningAsync(this IPageDialogService dialog, string warningMessage)
        {
            return dialog.DisplayAlertAsync(
                "Warning",
                warningMessage,
                "Yes",
                "No"
            );
        }
    }
}
