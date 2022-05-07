using System;
using System.Threading.Tasks;
using Prism.Navigation;

namespace Warehouse.Mobile.Extensions
{
    public static class NavigationServiceExtension
    {
        public static Task ShowSuccessAsync(
            this INavigationService navigationService,
            string message)
        {
            return navigationService.ShowMessageAsync(
                PopupSeverity.Info,
                "Success!",
                message
            );
        }

        public static Task ShowErrorAsync(
            this INavigationService _navigationService,
            Exception exception)
        {
            return _navigationService.ShowErrorAsync(
                exception.Message
            );
        }

        public static Task ShowErrorAsync(
            this INavigationService _navigationService,
            string errorMessage)
        {
            return _navigationService.ShowMessageAsync(
                PopupSeverity.Error,
                "Error!",
                errorMessage
            );
        }

        public static Task ShowWarningAsync(
            this INavigationService _navigationService,
            string  warningMessage)
        {
             return _navigationService.ShowMessageAsync(
                 PopupSeverity.Warning,
                 "Warning!",
                 warningMessage
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
