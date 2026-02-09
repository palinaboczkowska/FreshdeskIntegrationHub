namespace FreshdeskIntegrationHub
{
    public class FreshdeskOptions
    {
        public string Domain { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;

        // Sync interval in seconds
        public int SyncIntervalSeconds { get; set; } = 30;

    }
}
