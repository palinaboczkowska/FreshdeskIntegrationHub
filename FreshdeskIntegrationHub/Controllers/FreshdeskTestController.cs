using FreshdeskIntegrationHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreshdeskIntegrationHub.Controllers
{
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

    }

}
