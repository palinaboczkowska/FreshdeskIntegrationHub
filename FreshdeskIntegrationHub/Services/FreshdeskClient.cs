using FreshdeskIntegrationHub.Domain.Entities;
using FreshdeskIntegrationHub.Dto;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
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
            var response = await _httpClient.GetAsync("/api/v2/tickets");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<Ticket>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Ticket>();
        }


        public async Task<Ticket?> CreateTicketAsync(CreateTicketRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/v2/tickets", content);
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Freshdesk error {response.StatusCode}: {errorBody}");
        }


        var responseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Ticket>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<Ticket?> GetTicketByIdAsync(long id)
        {
            var response = await _httpClient.GetAsync($"/api/v2/tickets/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Freshdesk error {response.StatusCode}: {errorBody}");
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Ticket>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

}
