namespace backend.DTO;

public class OrderItemDto
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public double Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}