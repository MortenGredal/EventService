namespace EventService.Models;

public class OrderEvent
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public DateTime Timestamp { get; set; }
    public string EventData { get; set; }
}