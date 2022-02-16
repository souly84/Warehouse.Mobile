using System.Collections.Generic;
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
                    new MockEnvironmentConfig(new DataMocker.EnvironmentArgs
                    {
                        TestScenario = new List<string>(),
                        SharedFolderPath = new List<string>()
                    }),
                    typeof(MockDataComponent).Assembly
                ).GetMockerHandler()
            );
        }
    }
}
