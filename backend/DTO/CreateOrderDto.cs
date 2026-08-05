using System.Text.Json.Serialization;

namespace backend.DTO;

public class CreateOrderDto
{
    public int TableId { get; set; }
    public List<CreateOrderItemDto> Items { get; set; } = new();
    public DateTime OrderDate { get; set; } 
    
}

public class CreateOrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
