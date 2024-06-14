using Microsoft.AspNetCore.Mvc;
using MultiTenancyKeyedServicesTest.Models;
using MultiTenancyKeyedServicesTest.Services;

namespace MultiTenancyKeyedServicesTest.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TesterController : ControllerBase
    {
        private readonly ILogger<TesterController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TesterController(ILogger<TesterController> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public Result Get([FromQuery] string start, [FromQuery] string end)
        {
            var startingService = _serviceProvider.GetKeyedService<IStartingService>(start);
            var endingService = _serviceProvider.GetKeyedService<EndingService>(end);
            return new Result
            {
                Starting = (startingService as StartingService)!,
                Ending = endingService!
            };
        }
        [HttpGet]
        public async ValueTask<bool> AddAsync([FromQuery] string tenantId)
        {
            await RuntimeServiceProvider.GetServiceCollection()
               .AddKeyedSingleton<IStartingService, StartingService>(tenantId)
               .AddKeyedTransient<EndingService>(tenantId)
               .ReBuildAsync();
            return true;
        }
    }
}
