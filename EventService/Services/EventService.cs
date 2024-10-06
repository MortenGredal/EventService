using EventService.DbContexts;
using EventService.Models;
using Microsoft.EntityFrameworkCore;

namespace EventService.Services;

public class EventService(EventDbContext eventContext, ILogger<EventService> logger)
{
    public async Task<List<OrderEvent>> GetEvents(DateTime? since = null, int? from = null, int? to = null)
    {
        logger.LogInformation("Fetching events with filters - Since: {Since}, From: {From}, To: {To}",
            since, from, to);

        var query = eventContext.Events.AsQueryable();

        // Apply filters if they are provided
        if (since.HasValue)
        {
            query = query.Where(e => e.Timestamp >= since.Value);
        }

        if (from.HasValue)
        {
            query = query.Where(e => e.Id >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(e => e.Id <= to.Value);
        }

        // Always order by timestamp to ensure consistent results
        var events = await query
            .OrderBy(e => e.Timestamp)
            .ToListAsync();

        logger.LogInformation("Found {Count} events matching criteria", events.Count);
        return events;
    }
}