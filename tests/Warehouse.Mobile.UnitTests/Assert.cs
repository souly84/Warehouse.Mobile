using System.Threading.Tasks;
using MediaPrint;
using Newtonsoft.Json.Linq;
using WebRequest.Elegant.Extensions;
using Xunit.Abstractions;

namespace Warehouse.Mobile.UnitTests
{
    public class Assert : Xunit.Assert
    {
        public static async Task EqualJson<T>(
            string expectedJson,
            Task<T> actualJson,
            ITestOutputHelper output = null)
            where T : IPrintable
        {
            var actual = await actualJson.ConfigureAwait(false);
            EqualJson(
                expectedJson,
                actual.ToJson().ToString(),
                output
            );
        }

        public static void EqualJson(
            string expectedJson,
            string actualJson,
            ITestOutputHelper output = null)
        {
            expectedJson = expectedJson.NoNewLines();
            actualJson = actualJson.NoNewLines();
            JObject expected = JObject.Parse(expectedJson);
            JObject actual = JObject.Parse(actualJson);
            if (output != null)
            {
                output.WriteLine("Expected:" + expectedJson);
                output.WriteLine("Actual:" + actualJson);
            }

            Equal(expected, actual, JToken.EqualityComparer);
        }

        public static void EqualJsonArrays(
            string expectedJson,
            string actualJson,
            ITestOutputHelper output = null)
        {
            expectedJson = expectedJson.NoNewLines();
            actualJson = actualJson.NoNewLines();
            JArray expected = JArray.Parse(expectedJson);
            JArray actual = JArray.Parse(actualJson);
            if (output != null)
            {
                output.WriteLine("Expected:" + expectedJson);
                output.WriteLine("Actual:" + actualJson);
            }

            Equal(expected, actual, JToken.EqualityComparer);
        }
    }
}
