using System.Text.Json.Serialization;

namespace FreshdeskIntegrationHub.Models;

public class Ticket
{
    public long Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public int Status { get; set; }
    public int Priority { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

}

