using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace Warehouse.Mobile.Plugins
{
    public static class JObjectExtensions
    {
        public static T? Get<T>(this JObject jObject, [CallerMemberName] string? key = null)
            => jObject.Value<T>(key?.Substring(0, 1).ToLowerInvariant() + key?.Substring(1));

        public static T EnumValue<T>(this JToken token, string key)
            where T : struct
        {
            return (T)Enum.Parse(typeof(T), token.Value<string>(key));
        }

        public static DateTime DateTime(this JToken token, string key)
        {
            return System.DateTime.Parse(
                token.Value<string>(key),
                CultureInfo.InvariantCulture
            );
        }
    }
}
