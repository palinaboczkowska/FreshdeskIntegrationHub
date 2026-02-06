namespace FreshdeskIntegrationHub
{
    public class FreshdeskOptions
    {
        public string Domain { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public int PollingIntervalSeconds { get; set; } = 60;
    }
}
