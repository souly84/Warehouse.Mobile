using System.Linq;
using MediaPrint;

namespace Warehouse.Mobile.Extensions
{
    public static class DictionaryMediaExtensions
    {
        public static T? ValueOrDefault<T>(this DictionaryMedia dictionary, params string[] keys)
        {
            var targetKey = keys.FirstOrDefault(key => dictionary.Contains(key));
            if (targetKey != null)
            {
                return dictionary.Value<T>(targetKey);
            }
            return default;
        }
    }
}
