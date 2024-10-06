using EventService.DbContexts;
using EventService.Models;
using System.Threading.Channels;

namespace EventService.Services
{
    public class EventProcessingService(
        ChannelReader<PizzaOrder> channel,
        IServiceScopeFactory scopeFactory,
        ILogger<EventProcessingService> logger)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Event processing service starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // wait for a PizzaOrder to appear in the channel
                    var order = await channel.ReadAsync(stoppingToken);
                    logger.LogInformation("Processing order {OrderId}, for Table {TableNumber} and Pizza {PizzaNumber}", order.Id, order.TableNumber, order.PizzaNumber);

                    await ProcessOrderEventAsync(order, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // Normal shutdown
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing order event");
                }
            }
        }

        private async Task ProcessOrderEventAsync(PizzaOrder order, CancellationToken stoppingToken)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var eventContext = scope.ServiceProvider.GetRequiredService<EventDbContext>();

                var orderEvent = new OrderEvent
                {
                    OrderId = order.Id,
                    Timestamp = DateTime.UtcNow,
                    EventData = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        OrderId = order.Id,
                        order.TableNumber,
                        order.PizzaNumber,
                        order.OrderTime
                    })
                };

                eventContext.Events.Add(orderEvent);
                await eventContext.SaveChangesAsync(stoppingToken);

                logger.LogInformation("Created event for order {OrderId}", order.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process event for order {OrderId}", order.Id);
                throw; // Re-throw to be handled by the main loop
            }
        }
    }
}