namespace FreshdeskIntegrationHub.Models;

public class Ticket
{
    public int Id { get; set; } // Freshdesk ID
    public string Subject { get; set; }
    public int Status { get; set; }
    public int Priority { get; set; }
    public DateTime UpdatedAt { get; set; }
}

