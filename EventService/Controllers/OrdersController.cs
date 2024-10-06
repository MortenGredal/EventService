using EventService.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EventService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(OrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var orderId = await orderService.CreateOrder(request.TableNumber, request.PizzaNumber);
        return Ok(new { OrderId = orderId });
    }

    public class CreateOrderRequest
    {
        [Required]
        public int TableNumber { get; set; }

        [Required]
        public int PizzaNumber { get; set; }
    }
}