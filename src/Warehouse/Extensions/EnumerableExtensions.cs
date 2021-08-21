using System;
using System.Collections.Generic;

namespace Warehouse.Mobile.Plugins
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> items, Action<T> onItem)
        {
            foreach (var item in items)
            {
                onItem(item);
            }
            return items;
        }
    }
}
