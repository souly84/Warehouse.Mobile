using System;
using Prism.Navigation;

namespace Warehouse.Mobile.Extensions
{
    public static class NavigationParametersExtensions
    {
        public static void CheckMandatory(this INavigationParameters parameter, string key)
        {
            if (!parameter.ContainsKey(key))
            {
                throw new InvalidOperationException($"Mandatory param not provided: {key}");
            }
        }
    }
}
