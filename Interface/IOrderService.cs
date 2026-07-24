using backend.DTO;
using backend.Model;

namespace backend.Interface;

public interface IOrderService
{
    Task<Order> CreateOrder(Order order);

    Task<List<CreateOrderDto>> GetAllOrders();

    Task<Order?> GetOrderById(int id);

    Task UpdateOrder(int id);

    Task DeleteOrder(int id);
}