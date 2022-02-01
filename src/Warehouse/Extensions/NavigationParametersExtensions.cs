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

        public static T Value<T>(this INavigationParameters parameters, string key)
        {
            parameters.CheckMandatory(key);
            return parameters.GetValue<T>(key);
        }
    }
}
