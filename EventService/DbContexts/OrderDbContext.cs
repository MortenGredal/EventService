using EventService.Models;
using Microsoft.EntityFrameworkCore;

namespace EventService.DbContexts;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<PizzaOrder> Orders { get; set; }
}