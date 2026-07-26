namespace backend.DTO;

public class OrderListDTO
{
    public int nTable { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; }
    public List<string> ProductNames { get; set; }
    
    public List<decimal> UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
}