using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;
using Warehouse.Mobile.Plugins;

namespace Warehouse.Mobile.IntegrationTests
{
    public static class Asserts
    {
        public static void EqualJson(string expectedJson, string actualJson, ITestOutputHelper output = null)
        {
            JObject expected = JObject.Parse(expectedJson);
            JObject actual = JObject.Parse(actualJson);
            if (output != null)
            {
                output.WriteLine("Expected:" + expectedJson);
                output.WriteLine("Actual:" + actualJson);
            }

            Xunit.Assert.Equal(expected, actual, JToken.EqualityComparer);
        }

        public static void Equal(
            Exception exception,
            Dictionary<string, object> exceptionDictionaryToCheck)
        {
            var exceptionDictionary = exception.ToDictionary();
            foreach (var key in exceptionDictionary.Keys)
            {
                var expectedValue = exceptionDictionary[key]?.ToString();
                var actualValue = exceptionDictionaryToCheck[key]?.ToString();
                // it's a trick moment here because we cannot have the same stack trace
                if (key != "StackTrace")
                {
                    Xunit.Assert.Equal(expectedValue, actualValue);
                }
                else
                {
                    Xunit.Assert.NotEmpty(expectedValue);
                    Xunit.Assert.NotEmpty(actualValue);
                }
            }
        }
    }
}
