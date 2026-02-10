using System.Text.Json.Serialization;

namespace FreshdeskIntegrationHub.Dto;

public class AddNoteRequest
{
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    [JsonPropertyName("private")]
    public bool Private { get; set; } = false;
}
