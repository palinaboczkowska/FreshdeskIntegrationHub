using FreshdeskIntegrationHub;
using FreshdeskIntegrationHub.Dto;
using FreshdeskIntegrationHub.Services;
using Microsoft.Extensions.Options;
using System.Net;

public class FreshdeskClientTests
{

    [Fact]
    public async Task CreateTicketAsync_ReturnsTicket_WhenFreshdeskResponds200()
    {
        // Arrange
        var ticketJson = """
        {
            "id": 123,
            "subject": "Test ticket",
            "status": 2,
            "priority": 1
        }
        """;

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(ticketJson)
        };

        var handler = new MockHttpMessageHandler(response);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.com")
        };

        var options = Options.Create(new FreshdeskOptions
        {
            ApiKey = "dummy",
            Domain = "https://example.com"
        });

        var client = new FreshdeskClient(httpClient, options);

        var request = new CreateTicketRequest
        {
            Subject = "Test ticket"
        };

        // Act
        var result = await client.CreateTicketAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123, result.Id);
        Assert.Equal("Test ticket", result.Subject);
    }

    [Fact]
    public async Task CreateTicketAsync_ThrowsException_WhenFreshdeskReturnsError()
    {
        // Arrange
        var errorJson = """
    {
        "message": "Invalid request",
        "errors": [{ "field": "subject", "message": "cannot be blank" }]
    }
    """;

        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(errorJson)
        };

        var handler = new MockHttpMessageHandler(response);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.com")
        };

        var options = Options.Create(new FreshdeskOptions
        {
            ApiKey = "dummy",
            Domain = "https://example.com"
        });

        var client = new FreshdeskClient(httpClient, options);

        var request = new CreateTicketRequest
        {
            Subject = ""
        };

        // Act + Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => client.CreateTicketAsync(request));

        Assert.Contains("Freshdesk error", ex.Message);
        Assert.Contains("Invalid request", ex.Message);
    }

    [Fact]
    public async Task GetTicketByIdAsync_ReturnsTicket_WhenFreshdeskResponds200()
    {
        // Arrange
        var ticketJson = """
    {
        "id": 555,
        "subject": "Sample ticket",
        "status": 2,
        "priority": 1
    }
    """;

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(ticketJson)
        };

        var handler = new MockHttpMessageHandler(response);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.com")
        };

        var options = Options.Create(new FreshdeskOptions
        {
            ApiKey = "dummy",
            Domain = "https://example.com"
        });

        var client = new FreshdeskClient(httpClient, options);

        // Act
        var result = await client.GetTicketByIdAsync(555);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(555, result.Id);
        Assert.Equal("Sample ticket", result.Subject);
    }

    [Fact]
    public async Task GetTicketByIdAsync_ThrowsException_WhenFreshdeskReturnsError()
    {
        // Arrange
        var errorJson = """
    {
        "message": "Ticket not found"
    }
    """;

        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(errorJson)
        };

        var handler = new MockHttpMessageHandler(response);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.com")
        };

        var options = Options.Create(new FreshdeskOptions
        {
            ApiKey = "dummy",
            Domain = "https://example.com"
        });

        var client = new FreshdeskClient(httpClient, options);

        // Act + Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => client.GetTicketByIdAsync(999));

        Assert.Contains("Freshdesk error", ex.Message);
        Assert.Contains("Ticket not found", ex.Message);
    }

    [Fact]
    public async Task AddNoteAsync_ExecutesSuccessfully_WhenFreshdeskResponds200()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        var handler = new MockHttpMessageHandler(response);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.com")
        };

        var options = Options.Create(new FreshdeskOptions
        {
            ApiKey = "dummy",
            Domain = "https://example.com"
        });

        var client = new FreshdeskClient(httpClient, options);

        var request = new AddNoteRequest
        {
            Body = "Test note",
            Private = false
        };

        // Act + Assert
        await client.AddNoteAsync(123, request);
    }

    [Fact]
    public async Task AddNoteAsync_ThrowsException_WhenFreshdeskReturnsError()
    {
        // Arrange
        var errorJson = """
    {
        "message": "Invalid note"
    }
    """;

        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(errorJson)
        };

        var handler = new MockHttpMessageHandler(response);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.com")
        };

        var options = Options.Create(new FreshdeskOptions
        {
            ApiKey = "dummy",
            Domain = "https://example.com"
        });

        var client = new FreshdeskClient(httpClient, options);

        var request = new AddNoteRequest
        {
            Body = "",
            Private = false
        };

        // Act + Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => client.AddNoteAsync(123, request));

        Assert.Contains("Freshdesk error", ex.Message);
        Assert.Contains("Invalid note", ex.Message);
    }

}