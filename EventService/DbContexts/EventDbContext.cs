using EventService.Models;
using Microsoft.EntityFrameworkCore;

namespace EventService.DbContexts;

public class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options)
{
    public DbSet<OrderEvent> Events { get; set; }
}