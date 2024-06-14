using MultiTenancyKeyedServicesTest.Services;

namespace MultiTenancyKeyedServicesTest.Models
{
    public sealed class Result
    {
        public StartingService? Starting { get; init; }
        public EndingService? Ending { get; init; }
    }
}
