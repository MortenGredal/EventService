namespace EventService.Models
{
    public class PizzaOrder
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int PizzaNumber { get; set; }
        public DateTime OrderTime { get; set; }

    }
}
