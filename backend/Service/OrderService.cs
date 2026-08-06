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
    private readonly ITableService _tableService;

    public OrderService(
        IRepository<Order> orderRepository,
        IProductService productService,
        IOrderItemService orderItemService,
        ITableService tableService)
    {
        _orderRepository = orderRepository;
        _orderItemService = orderItemService;
        _productService = productService;
        _tableService = tableService;
    }

    public async Task<Order> CreateOrder(CreateOrderDto dto)
    {
        var order = new Order
        {
            TableId = dto.TableId,
            OrderDate = DateTime.Now.Date,
            OrderStatus = OrderStatus.Processing,
            Items = new List<OrderItem>()
        };
        
        foreach (var itemDto in dto.Items)
        {
            var products = await _productService.GetProductByIdAsync(itemDto.ProductId);
            
            var item = new OrderItem
            {
                OrderId = order.Id,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = products.Price
            };
            
            order.Items.Add(item);
        }
        
        order.TotalAmount = order.Items.Sum(x => (decimal)x.Quantity * x.UnitPrice);
        
        await _orderRepository.AddAsync(order);
        await _tableService.UpdateStatusTable(order.TableId, TableStatus.Occupied); 
        
        _orderRepository.Update(order);
        
        return order;
    }

    public async Task<List<OrderListDTO>> GetAllOrders()
    { 
        return await BaseOrderQuery(_orderRepository.GetAllAsync()).ToListAsync();
    }

    public async Task<List<OrderListDTO>> GetOrdersDay()
    {
        var today = DateTime.Today;
        return await BaseOrderQuery(_orderRepository.GetAllAsync().Where(x => x.OrderDate == today)).ToListAsync();
    }

    private IQueryable<OrderListDTO> BaseOrderQuery(IQueryable<Order> query)
    {
        return query.Include(t => t.Table)
            .Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .OrderBy(t => t.OrderDate)
            .ThenBy(t => t.OrderStatus)
            .Select(o => new OrderListDTO
            {
                id = o.Id,
                TableId = o.TableId,
                nTable = o.Table.Number,
                Quantities = o.Items.Select(x => x.Quantity).ToList(),
                OrderDate = o.OrderDate,
                OrderStatus = o.OrderStatus,
                ProductNames = o.Items.Select(x => x.Product.Name).ToList(),
                UnitPrice = o.Items.Select(x => x.UnitPrice).ToList(),
                TotalAmount = o.TotalAmount
            });
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task DeleteOrder(int id)
    {
        await _orderRepository.DeleteAsync(id);
    }
    public async Task<Order> UpdateOrder(int idOrder, OrderStatus status)
    {
        var order = await _orderRepository.GetByIdAsync(idOrder);
        order.OrderStatus = status;
        _orderRepository.Update(order);

        if (status == OrderStatus.Finalized)
        {
            var orderItems = await _orderRepository
                .GetAllAsync()
                .Where(o => o.TableId == order.TableId)
                .ToListAsync();
            
            bool allFinalized = orderItems.All(x => x.OrderStatus == OrderStatus.Finalized);
            

            if (allFinalized)
            {
                await _tableService.UpdateStatusTable(order.TableId, TableStatus.Available);
            }
        }
        
        if(status != OrderStatus.Finalized)
        {
            await _tableService.UpdateStatusTable(order.TableId, TableStatus.Occupied);
        }

        var itemsOrders = await _orderItemService.GetOrderItemsByOrderId(idOrder);
        
        order.TotalAmount = itemsOrders.Sum(x => x.Quantity * x.UnitPrice);
        
        _orderRepository.Update(order);
        
        return order;
    }
    
}