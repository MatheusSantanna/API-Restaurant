using backend.DTO;
using backend.Model;

namespace backend.Interface;

public interface IOrderItemService
{
    Task<OrderItemDto> CreateOrderItem(OrderItemDto dto);

    Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId);

    Task<List<OrderItemDto>> GetAllOrderItems();

    Task<OrderItem?> GetOrderItemById(int id);
}