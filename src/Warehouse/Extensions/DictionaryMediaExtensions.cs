using MediaPrint;

namespace Warehouse.Mobile.Extensions
{
    public static class DictionaryMediaExtensions
    {
        public static T ValueOrDefault<T>(this DictionaryMedia dictionary, params string[] keys)
        {
            foreach (var key in keys)
            {
                if (dictionary.Contains(key))
                {
                    return dictionary.Value<T>(key);
                }
            }
            return default;
        }
    }
}
