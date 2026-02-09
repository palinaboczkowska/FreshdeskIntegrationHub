using FreshdeskIntegrationHub;
using FreshdeskIntegrationHub.Data;
using FreshdeskIntegrationHub.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.Configure<FreshdeskOptions>(
    builder.Configuration.GetSection("Freshdesk")
);
builder.Services.AddHttpClient<FreshdeskClient>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<FreshdeskSyncService>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=freshdesk.db"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();
