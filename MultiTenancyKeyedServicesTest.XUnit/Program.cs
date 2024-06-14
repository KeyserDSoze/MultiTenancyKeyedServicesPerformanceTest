using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenancyKeyedServicesTest.Controllers;
using MultiTenancyKeyedServicesTest.Extensions;
using Rystem.Test.XUnit;

namespace MultiTenancyKeyedServicesTest.XUnit
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [RPlotExporter]
    internal class Startup : StartupHelper
    {
        protected override string? AppSettingsFileName => "appsettings.json";
        protected override bool HasTestHost => true;
        protected override bool AddHealthCheck => true;
        protected override Type? TypeToChooseTheRightAssemblyToRetrieveSecretsForConfiguration => typeof(Startup);

        protected override Type? TypeToChooseTheRightAssemblyWithControllersToMap => typeof(TesterController);
        [Benchmark]
        protected override IServiceCollection ConfigureClientServices(IServiceCollection services)
        {
            services.AddHttpClient("client", x =>
            {
                x.BaseAddress = new Uri("http://localhost");
            });
            return services;
        }
        [Benchmark]
        protected override ValueTask ConfigureServerServicesAsync(IServiceCollection services, IConfiguration configuration)
        {
            var numberOfTenants = int.Parse(configuration["Settings:NumberOfTenants"]!);
            var numberOfServicesForTenant = int.Parse(configuration["Settings:NumberOfServicesForTenant"]!);
            services.AddTestServices(numberOfTenants, numberOfServicesForTenant);
            return ValueTask.CompletedTask;
        }
        [Benchmark]
        protected override ValueTask ConfigureServerMiddlewareAsync(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            applicationBuilder.UseTestMiddlewares();
            return ValueTask.CompletedTask;
        }
    }
}
