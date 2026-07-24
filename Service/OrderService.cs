using backend.DTO;
using backend.Interface;
using backend.Model;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace backend.Service;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly ITableService _tableService;
    private readonly IOrderItemService _orderItemService;

    public OrderService(
        IRepository<Order> orderRepository,
        ITableService tableService,
        IOrderItemService orderItemService)
    {
        _orderRepository = orderRepository;
        _tableService = tableService;
        _orderItemService = orderItemService;
    }

    public async Task<Order> CreateOrder(Order order)
    {
        await _orderRepository.AddAsync(order);

        foreach (var itemDto in order.Items)
        {
            var item = new OrderItemDto
            {
                OrderId = order.Id,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice
            };
            
            await _orderItemService.CreateOrderItem(item);
        }
        
        order.TotalAmount = order.Items.Sum(x => (decimal)x.Quantity * x.UnitPrice);
        
        _orderRepository.Update(order);
        
        return order;
    }

    public async Task<List<CreateOrderDto>> GetAllOrders()
    {
        return await _orderRepository
            .GetAllAsync()
            .OrderBy(x => x.OrderDate)
            .ThenBy(x => x.OrderStatus)
            .Select(x => new CreateOrderDto
            {
                TableId = x.TableId,
                OrderDate = x.OrderDate,
                OrderStatus = x.OrderStatus,
                TotalAmount = x.TotalAmount
            })
            .ToListAsync();
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task DeleteOrder(int id)
    {
        await _orderRepository.DeleteAsync(id);
    }

    public async Task UpdateOrder(int id)
    {
        var order = await GetOrderById(id);

        if (order == null)
            throw new KeyNotFoundException("Order not found.");

        var orderItems = await _orderItemService.GetOrderItemsByOrderId(id);

        order.TotalAmount = orderItems.Sum(x => (decimal)x.Quantity * x.UnitPrice);

        _orderRepository.Update(order);
    }
}