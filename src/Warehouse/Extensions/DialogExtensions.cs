using System.Threading.Tasks;
using Prism.Services;

namespace Warehouse.Mobile
{
    public static class DialogExtensions
    {
        public static Task ErrorAsync(this IPageDialogService dialog, string errorMessage)
        {
            return dialog.DisplayAlertAsync(
                "Error",
                errorMessage,
                "Ok"
            );
        }
    }
}
