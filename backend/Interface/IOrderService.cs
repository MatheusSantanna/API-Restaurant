using backend.DTO;
using backend.Model;

namespace backend.Interface;

public interface IOrderService
{
    Task<Order> CreateOrder(CreateOrderDto order);

    Task<List<OrderListDTO>> GetAllOrders();

    Task<Order?> GetOrderById(int id);

    Task<Order> UpdateOrder(int idOrder, OrderStatus status);

    Task DeleteOrder(int id);
    Task<List<OrderListDTO>> GetOrdersDay();
}