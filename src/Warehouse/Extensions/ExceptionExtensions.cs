using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Warehouse.Mobile.Plugins
{
    public static class ExceptionExtensions
    {
        public static Exception Unwrap(this Exception exception)
        {
            Exception ex = exception;
            while (ex is AggregateException && ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }

        public static Dictionary<string, object> ToDictionary(this Exception exc)
        {
            return new Dictionary<string, object>
            {
                ["ClassName"] = exc.GetType().AssemblyQualifiedName,
                ["Message"] = exc.Message,
                ["Data"] = exc.Data.Count > 0
                    ? JsonConvert.SerializeObject(exc.Data)
                    : null,
                ["InnerException"] = exc.InnerException?.ToJson(),
                ["StackTrace"] = exc.StackTrace,
                ["HResult"] = exc.HResult,
                ["Source"] = exc.Source,
            };
        }

        public static string ToJson(this Exception exc)
        {
            return JsonConvert.SerializeObject(
                exc.ToDictionary(),
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                }
            );
        }

        public static Exception ToException(this string exceptionJson)
        {
            var parsedException = JsonConvert.DeserializeObject<JObject>(exceptionJson);
            return parsedException
                .WithInnerException()
                .WithStackTrace(parsedException);
        }

        public static IList<string> ToErrorMessages(this Exception exeption)
        {
            var errors = new List<string>();
            while (exeption != null)
            {
                if (!errors.Contains(exeption.Message))
                {
                    errors.Add(exeption.Message);
                }
                exeption = exeption.InnerException;
            }
            return errors;
        }

        private static Exception WithStackTrace(this Exception exception, JObject parsedException)
        {
            if (parsedException.ContainsKey("StackTrace"))
            {
                typeof(Exception)
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .First(x => x.Name.Contains("StackTraceString"))
                    .SetValue(exception, parsedException["StackTrace"].Value<string>());
            }

            return exception;
        }

        private static Exception ToException(this JObject parsedException)
        {
            parsedException.FixIncorrectExceptionProperties();
            var excpetionType = Type.GetType(
                parsedException["ClassName"].Value<string>(),
                false
            );
            if (excpetionType != null)
            {
                // https://github.com/dotnet/runtime/issues/42154 issue
                if (excpetionType == typeof(HttpRequestException))
                {
                    return new HttpRequestException(parsedException["Message"].Value<string>());
                }
                try
                {
                    return (Exception)parsedException.ToObject(excpetionType);
                }
                catch (Exception)
                {
                    // Nothing to do here, just return UnknownExceptionTypeException if was not able to parse.
                }
            }

            return new UnknownExceptionTypeException(
                   parsedException["Message"].Value<string>(),
                   parsedException["ClassName"].Value<string>()
               );
        }

        private static Exception WithInnerException(this JObject parsedException)
        {
            var innerException = parsedException["InnerException"]?.ToString();
            parsedException["InnerException"] = null;
            var exception = parsedException.ToException();
            if (!string.IsNullOrEmpty(innerException))
            {
                var ex = innerException.ToException();
                typeof(Exception)
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .First(x => x.Name.Contains("_innerException"))
                    .SetValue(exception, ex);
            }
            return exception;
        }

        private static void FixIncorrectExceptionProperties(this JObject parsedException)
        {
            if (parsedException["Data"].ToString() == "{}")
            {
                parsedException["Data"] = null;
            }
            parsedException.AddNeededProperties(
                "HelpURL",
                "StackTraceString",
                "RemoteStackTraceString"
            );
            if (!parsedException.ContainsKey("RemoteStackIndex"))
            {
                parsedException["RemoteStackIndex"] = 0;
            }
            if (!parsedException.ContainsKey("NativeErrorCode"))
            {
                parsedException["NativeErrorCode"] = 0;
            }
        }

        private static void AddNeededProperties(this JObject parsedException, params string[] properties)
        {
            foreach (var property in properties)
            {
                if (!parsedException.ContainsKey(property))
                {
                    parsedException[property] = null;
                }
            }
        }
    }
}
