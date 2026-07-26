using backend.DTO;
using backend.Model;

namespace backend.Interface;

public interface IOrderService
{
    Task<Order> CreateOrder(CreateOrderDto order);

    Task<List<OrderListDTO>> GetAllOrders();

    Task<Order?> GetOrderById(int id);

    Task UpdateOrder(int id);

    Task DeleteOrder(int id);
}