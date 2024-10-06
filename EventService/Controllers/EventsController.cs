using EventService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(Services.EventService eventService, ILogger<EventsController> _logger) : ControllerBase
{
    /// <summary>
    /// Get events with optional filtering
    /// </summary>
    /// <param name="since">Optional timestamp to filter events from</param>
    /// <param name="from">Optional starting event ID</param>
    /// <param name="to">Optional ending event ID</param>
    /// <returns>List of events matching the criteria</returns>
    [HttpGet]
    public async Task<IActionResult> GetEvents(
        [FromQuery] DateTime? since = null,
        [FromQuery] int? from = null,
        [FromQuery] int? to = null)
    {
        try
        {
            // Validate that if 'to' is provided, it's greater than or equal to 'from'
            if (from.HasValue && to.HasValue && to.Value < from.Value)
            {
                return BadRequest("'to' must be greater than or equal to 'from'");
            }

            var events = await eventService.GetEvents(since, from, to);
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching events");
            return StatusCode(500, "An error occurred while fetching events");
        }
    }
}