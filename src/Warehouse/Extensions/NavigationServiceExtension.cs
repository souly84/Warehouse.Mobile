using System;
using System.Threading.Tasks;
using Prism.Navigation;

namespace Warehouse.Mobile.Extensions
{
    public static class NavigationServiceExtension
    {
        public static Task ShowErrorAsync(
            this INavigationService _navigationService,
            Exception exception)
        {
            return _navigationService.ShowMessageAsync(
                PopupSeverity.Error,
                "Error!",
                exception.Message
            );
        }

        public static async Task ShowMessageAsync(
            this INavigationService _navigationService,
            PopupSeverity severity,
            string title,
            string message)
        {
            var completionToken = new TaskCompletionSource<bool>();
            Action<bool, Exception?> callBack = (result, excpetion) =>
            {
                if (excpetion != null)
                {
                    completionToken.SetException(excpetion);
                }
                completionToken.SetResult(result);
            };
            var result = await _navigationService.NavigateAsync(
                AppConstants.CustomPopupMessageViewId,
                new NavigationParameters
                {
                    { "Severity", severity},
                    { "Title", title},
                    { "Message", message},
                    { "ActionText", "GOT IT!"},
                    { "CallBack", callBack }
                }
            );
            if (result.Exception != null)
            {
                throw result.Exception;
            }
            await completionToken.Task;
        }
    }
}
