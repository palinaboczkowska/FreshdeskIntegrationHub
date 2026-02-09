using FreshdeskIntegrationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace FreshdeskIntegrationHub.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<SyncState> SyncStates { get; set; }
}

