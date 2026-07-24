using backend.DTO;
using backend.Interface;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderItemController : ControllerBase
{
    private readonly IOrderItemService _orderItemService;

    public OrderItemController(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrderItems()
    {
        var orderItems = await _orderItemService.GetAllOrderItems();

        return Ok(orderItems);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemDto orderItemDto)
    {
        await _orderItemService.CreateOrderItem(orderItemDto);

        return Ok(orderItemDto);
    }
}