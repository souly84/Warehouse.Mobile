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
            Func<Task<INavigationResult?>> onNavigation,
            [CallerMemberName] string? name = null)
        {
            return commands.AsyncCommand(() =>
            {
                return Device.InvokeOnMainThreadAsync(async () =>
                {
                    var navigationResult = await onNavigation();
                    _ = navigationResult ?? throw new InvalidOperationException("Navigation result can not be null");
                    if (navigationResult.Exception != null)
                    {
                        throw navigationResult.Exception;
                    }
                });
            }, name: name, forceExecution : true);
        }
    }
}
