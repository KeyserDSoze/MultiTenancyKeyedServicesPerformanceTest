namespace MultiTenancyKeyedServicesTest.Services
{
    public interface IStartingService
    {
        string A { get; }
    }
    public class StartingService : IStartingService
    {
        public string A { get; set; } = Guid.NewGuid().ToString();
    }
    public class EndingService
    {
        public string B { get; set; } = Guid.NewGuid().ToString();
    }
}
