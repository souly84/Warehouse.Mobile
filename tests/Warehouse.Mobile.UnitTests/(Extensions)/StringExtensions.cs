using WebRequest.Elegant;
using WebRequest.Elegant.Fakes;

namespace Warehouse.Mobile.UnitTests.Extensions
{
    public static class StringExtensions
    {
        public static IWebRequest JsonAsWebRequest(this string json)
        {
            return new WebRequest.Elegant.WebRequest(
                "http://fake.server.for.unittests.com",
                new FkHttpMessageHandler(json)
            );
        }
    }
}
