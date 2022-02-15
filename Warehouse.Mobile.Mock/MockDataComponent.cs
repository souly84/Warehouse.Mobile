using System.Net.Http;
using DataMocker.Mock;

namespace Warehouse.Mobile.Mock
{
    public class MockDataComponent
    {
        public static HttpClient HttpClient()
        {
            return new HttpClient(
                new MockHandlerInitializer(
                    "",
                    typeof(MockDataComponent).Assembly
                ).GetMockerHandler()
            );
        }
    }
}
