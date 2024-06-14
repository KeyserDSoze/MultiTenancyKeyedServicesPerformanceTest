using System.Net.Http.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MultiTenancyKeyedServicesTest.Models;

namespace MultiTenancyKeyedServicesTest.XUnit
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [RPlotExporter]
    public class EndpointTest
    {
        private readonly HttpClient _httpClient;
        public EndpointTest(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("client");
        }
        [Benchmark]
        [Theory]
        [InlineData("1", "3")]
        public async ValueTask ReturnServicesAsync(string start, string end)
        {
            var response = await _httpClient.GetAsync($"tester/get?start={start}&end={end}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<Result>();
            Assert.NotNull(result);
            Assert.NotNull(result.Starting);
            Assert.NotNull(result.Ending);
        }
        [Benchmark]
        [Fact]
        public async ValueTask AddNewTenantAsync()
        {
            var tenantId = Guid.NewGuid().ToString("N");
            var response = await _httpClient.GetAsync($"tester/add?tenantId={tenantId}");
            response.EnsureSuccessStatusCode();
            var isOk = await response.Content.ReadFromJsonAsync<bool>();
            Assert.True(isOk);
            response = await _httpClient.GetAsync($"tester/get?start={tenantId}&end={tenantId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<Result>();
            Assert.NotNull(result);
            Assert.NotNull(result.Starting);
            Assert.NotNull(result.Ending);
        }
    }
}
