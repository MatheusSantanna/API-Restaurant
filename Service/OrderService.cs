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
    private readonly IOrderItemService _orderItemService;
    private readonly IProductService _productService;

    public OrderService(
        IRepository<Order> orderRepository,
        IProductService productService,
        IOrderItemService orderItemService)
    {
        _orderRepository = orderRepository;
        _orderItemService = orderItemService;
        _productService = productService;
    }

    public async Task<Order> CreateOrder(CreateOrderDto dto)
    {
        var order = new Order
        {
            TableId = dto.TableId,
            OrderDate = DateTime.Now.Date,
            OrderStatus = "Aberto",
            Items = new List<OrderItem>()
        };
        
        await _orderRepository.AddAsync(order);
        
        foreach (var itemDto in dto.Items)
        {
            var products = await _productService.GetProductByIdAsync(itemDto.ProductId);
            
            var item = new OrderItemDto
            {
                OrderId = order.Id,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                Price = products.Price
            };
            
            await _orderItemService.CreateOrderItem(item);
        }
        
        order.TotalAmount = order.Items.Sum(x => (decimal)x.Quantity * x.UnitPrice);
        
        _orderRepository.Update(order);
        
        return order;
    }

    public async Task<List<OrderListDTO>> GetAllOrders()
    {
        // retirar os parametros que nao iremos usar e utilizar somente o mesa e o nome dos produtos
        return await _orderRepository
            .GetAllAsync()
            .Include(t => t.Table)
            .Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .OrderBy(t => t.OrderDate)
            .ThenBy(t => t.OrderStatus)
            .Select(o => new OrderListDTO
            {
                nTable = o.Table.Number,
                OrderDate = o.OrderDate,
                OrderStatus = o.OrderStatus,
                ProductNames = o.Items.Select(x => x.Product.Name).ToList(),
                UnitPrice = o.Items.Select(x => x.UnitPrice).ToList(),
                TotalAmount = o.TotalAmount
            }).ToListAsync();
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