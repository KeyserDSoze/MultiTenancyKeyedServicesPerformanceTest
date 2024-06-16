using Microsoft.AspNetCore.Mvc;
using MultiTenancyKeyedServicesTest.Models;
using MultiTenancyKeyedServicesTest.Services;
using RepositoryFramework;

namespace MultiTenancyKeyedServicesTest.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TesterController : ControllerBase
    {
        private readonly ILogger<TesterController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFactory<IRepository<SomeRepositoryModel, string>> _factory;

        public TesterController(ILogger<TesterController> logger,
            IServiceProvider serviceProvider,
            IFactory<IRepository<SomeRepositoryModel, string>> factory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _factory = factory;
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
        [HttpGet]
        public async Task<bool> CheckFactoryAsync([FromQuery] string tenantId)
        {
            var repo = _factory.Create(tenantId);
            var id = Guid.NewGuid().ToString();
            var result = await repo!.InsertAsync(id, new SomeRepositoryModel
            {
                Id = id,
                Name = "example"
            });
            return result.IsOk;
        }
    }
}
