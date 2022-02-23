using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dotnet.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace Warehouse.Mobile.Extensions
{
    public static class CommandsExtensions
    {
        public static IAsyncCommand NavigationCommand(
            this CachedCommands commands,
            Func<Task<INavigationResult>> onNavigation,
            [CallerMemberName] string? name = null)
        {
            return commands.AsyncCommand(() =>
            {
                var taskCompletionSource = new TaskCompletionSource<bool>();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        var navigationResult = await onNavigation();
                        if (navigationResult.Exception != null)
                        {
                            throw navigationResult.Exception;
                        }
                        taskCompletionSource.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        taskCompletionSource.SetException(ex);
                    }
                });
                return taskCompletionSource.Task;
            }, name: name, forceExecution : true);
        }
    }
}
