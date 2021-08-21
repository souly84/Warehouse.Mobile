using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Warehouse.Server.FakeWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly ILogger<WarehouseController> _logger;

        public WarehouseController(ILogger<WarehouseController> logger)
        {
            _logger = logger;
        }

        [HttpGet(nameof(Test))]
        public async Task<string> Test()
        {
            return "Test successful";
        }
    }
}
