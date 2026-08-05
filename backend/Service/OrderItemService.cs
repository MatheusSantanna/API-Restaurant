using backend.DTO;
using backend.Interface;
using backend.Model;
using backend.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class OrderItemService : IOrderItemService
{
    private readonly IRepository<OrderItem> _repository;
    
    

    public OrderItemService(
        IRepository<OrderItem> repository)
    {
        _repository = repository;
    }

    public async Task<OrderItemDto> CreateOrderItem(OrderItemDto dto)
    {
        var orderItem = new OrderItem
        {
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.Price
        };

        await _repository.AddAsync(orderItem);
        
        return dto;
    }

    public async Task<List<OrderItemDto>> GetAllOrderItems()
    {
        return await _repository.GetAllAsync()
            .OrderBy(x => x.Id)
            .ThenBy(x => x.OrderId)
            .Select(x => new OrderItemDto
            {
                OrderId = x.OrderId,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Price = x.UnitPrice
            })
            .ToListAsync();
    }

    public async Task<OrderItem?> GetOrderItemById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId)
    {
        return await _repository
            .GetAllAsync()
            .Where(x => x.OrderId == orderId)
            .ToListAsync();
    }
}