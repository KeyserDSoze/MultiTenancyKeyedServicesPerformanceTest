using MultiTenancyKeyedServicesTest.Models;
using MultiTenancyKeyedServicesTest.Services;
using RepositoryFramework;
using RepositoryFramework.InMemory;
using System.Reflection;

namespace MultiTenancyKeyedServicesTest.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTestServices(this IServiceCollection services, int numberOfTenants, int numberOfServicesForTenant)
        {
            services.AddRuntimeServiceProvider();
            List<Type> servicesToAdd = [];
            var startingType = typeof(StartingService);
            for (var j = 0; j < numberOfServicesForTenant; j++)
            {
                var mockedType = startingType.Mock((configuration) =>
                {
                    configuration.IsSealed = false;
                    configuration.CreateNewOneIfExists = true;
                })!;
                servicesToAdd.Add(mockedType!);
            }
            for (var i = 0; i < numberOfTenants; i++)
            {
                var tenantId = i.ToString();
                services.AddKeyedSingleton<IStartingService, StartingService>(tenantId);
                services.AddKeyedTransient<EndingService>(tenantId);
                for (var j = 0; j < numberOfServicesForTenant; j++)
                {
                    services.AddKeyedScoped(servicesToAdd[j], tenantId);
                }
            }
            services.AddRepository<SomeRepositoryModel, string>(repoBuilder =>
            {
                repoBuilder.WithInMemory(inMemoryBuilder =>
                {
                    inMemoryBuilder.PopulateWithRandomData();
                }, "example");
            });
            services.AddActionAsFallbackWithServiceCollectionRebuilding<IRepository<SomeRepositoryModel, string>>(async fallbackBuilder =>
            {
                fallbackBuilder.Services.AddRepository<SomeRepositoryModel, string>(repoBuilder =>
                {
                    repoBuilder.WithInMemory(inMemoryBuilder =>
                    {
                        inMemoryBuilder.PopulateWithRandomData();
                    }, fallbackBuilder.Name);
                });
            });
            return services;
        }
    }
}
