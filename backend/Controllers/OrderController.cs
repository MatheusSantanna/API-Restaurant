using backend.DTO;
using backend.Interface;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderService.GetAllOrders();

        return Ok(orders);
    }
    
    [HttpGet("today")]
    public async Task<IActionResult> GetOrdersDay()
    {
        var ordersDay = await _orderService.GetOrdersDay();
        return Ok(ordersDay);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderById(id);

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto order)
    {
        await _orderService.CreateOrder(order);

        return Ok(order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id,[FromBody] OrderStatus status)
    {
        var order = await _orderService.UpdateOrder(id, status);
        
        if (order == null)
            return NotFound();
        
        return Ok(order);
    }

    [HttpPatch("{id}/close")] 
    public async Task<IActionResult> ClosesOrder(int id)
    {
        await _orderService.UpdateOrder(id, OrderStatus.Finalized);
        
        
        return Ok();
    }


}