using FreshdeskIntegrationHub.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FreshdeskIntegrationHub.Services;
    public class FreshdeskClient
    {
        private readonly HttpClient _httpClient;
        private readonly FreshdeskOptions _options;

        public FreshdeskClient(HttpClient httpClient, IOptions<FreshdeskOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            _httpClient.BaseAddress = new Uri($"https://{_options.Domain}/");

            var auth = Convert.ToBase64String(
                System.Text.Encoding.ASCII.GetBytes($"{_options.ApiKey}:X")
            );

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", auth);
        }

        public async Task<string> GetTicketsRawAsync()
        {
            var response = await _httpClient.GetAsync("/api/v2/tickets");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    // Parsed ticket list for sync service
        public async Task<List<Ticket>> GetTicketsAsync()
        {
            var json = await GetTicketsRawAsync();

            // Deserialize Freshdesk tickets
            var tickets = JsonSerializer.Deserialize<List<Ticket>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return tickets ?? new List<Ticket>();
        }

}
