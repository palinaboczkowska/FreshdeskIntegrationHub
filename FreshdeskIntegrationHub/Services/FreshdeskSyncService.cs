using FreshdeskIntegrationHub.Data;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using FreshdeskIntegrationHub.Domain.Entities;


namespace FreshdeskIntegrationHub.Services;

public class FreshdeskSyncService : BackgroundService
{
    private readonly ILogger<FreshdeskSyncService> _logger;
    private readonly FreshdeskClient _client;
    private readonly IServiceProvider _serviceProvider;
    private readonly FreshdeskOptions _options;

    public FreshdeskSyncService(
        ILogger<FreshdeskSyncService> logger,
        FreshdeskClient client,
        IServiceProvider serviceProvider,
        IOptions<FreshdeskOptions> options)
    {
        _logger = logger;
        _client = client;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Freshdesk Sync Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SyncTicketsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Freshdesk sync");
            }

            await Task.Delay(TimeSpan.FromSeconds(_options.SyncIntervalSeconds), stoppingToken);
        }
    }

    private async Task SyncTicketsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        _logger.LogInformation("Starting Freshdesk sync...");

        // 1. Fetch tickets from Freshdesk API
        var tickets = await _client.GetTicketsAsync();

        // 2. Load last sync timestamp
        var syncState = await db.SyncStates.FirstOrDefaultAsync();
        var lastSync = syncState?.LastSyncTime ?? DateTime.MinValue;

        // 3. Filter only updated or new tickets
        var updatedTickets = tickets.Where(t => t.UpdatedAt > lastSync).ToList();
        //var updatedTickets = tickets.ToList();


        foreach (var ticket in updatedTickets)
        {
            var existing = await db.Tickets.FindAsync(ticket.Id);

            if (existing == null)
            {
                // New ticket - insert
                db.Tickets.Add(ticket);
            }
            else
            {
                // Existing ticket - update fields
                existing.Subject = ticket.Subject;
                existing.Status = ticket.Status;
                existing.Priority = ticket.Priority;
                existing.UpdatedAt = ticket.UpdatedAt;
            }
        }

        // 4. Update last sync timestamp
        if (syncState == null)
        {
            syncState = new SyncState { LastSyncTime = DateTime.UtcNow };
            db.SyncStates.Add(syncState);
        }
        else
        {
            syncState.LastSyncTime = DateTime.UtcNow;
        }

        await db.SaveChangesAsync();

        _logger.LogInformation("Freshdesk sync completed");
    }
}