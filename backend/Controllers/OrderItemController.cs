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
    private readonly IOrderService _orderService;

    public OrderItemController(IOrderItemService orderItemService,
        IOrderService orderService)
    {
        _orderItemService = orderItemService;
        _orderService = orderService;
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
        await _orderService.UpdateOrder(orderItemDto.OrderId, OrderStatus.Processing);

        return Ok(orderItemDto);
    }
}