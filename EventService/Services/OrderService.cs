using EventService.DbContexts;
using EventService.Models;
using System.Threading.Channels;

namespace EventService.Services;

public class OrderService(
    OrderDbContext orderContext,
    ChannelWriter<PizzaOrder> channel,
    ILogger<OrderService> logger)
{

    public async Task<int> CreateOrder(int tableNumber, int pizzaNumber)
    {
        // Save the order
        var order = new PizzaOrder
        {
            TableNumber = tableNumber,
            PizzaNumber = pizzaNumber,
            OrderTime = DateTime.UtcNow
        };

        orderContext.Orders.Add(order);
        await orderContext.SaveChangesAsync();

        // Queue the order for event processing
        logger.LogInformation("Queuing order {OrderId} for event processing", order.Id);
        await channel.WriteAsync(order);

        return order.Id;
    }
}