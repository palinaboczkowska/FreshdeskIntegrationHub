using FreshdeskIntegrationHub.Dto;
using FreshdeskIntegrationHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreshdeskIntegrationHub.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class FreshdeskTestController : ControllerBase
    {
        private readonly FreshdeskClient _client;

        public FreshdeskTestController(FreshdeskClient client)
        {
            _client = client;
        }

        [HttpGet("tickets")]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _client.GetTicketsAsync();
            return Ok(tickets);
        }

        [HttpPost("tickets")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest request)
        {
            var ticket = await _client.CreateTicketAsync(request);
            return Ok(ticket);
        }

        [HttpGet("tickets/{id}")]
        public async Task<IActionResult> GetTicketById(long id)
        {
            var ticket = await _client.GetTicketByIdAsync(id);
            return Ok(ticket);
        }

        [HttpPost("tickets/{id}/notes")]
        public async Task<IActionResult> AddNote(long id, [FromBody] AddNoteRequest request)
        {
            await _client.AddNoteAsync(id, request);
            return Ok(new { message = "Note added successfully" });
        }
}


