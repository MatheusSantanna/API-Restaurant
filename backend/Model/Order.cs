using backend.DTO;

namespace backend.Model
{
    public class Order
    {
        public int Id { get; set; }

        public int TableId { get; set; }

        public DateTime OrderDate { get; set; }
        
        public decimal TotalAmount { get; set; }

        public virtual Table Table { get; set; }

        public virtual List<OrderItem> Items { get; set; } = new();
        
        public OrderStatus OrderStatus { get; set; } 
    }
    
    public enum OrderStatus
    {
        Processing,
        Complete,
        Finalized
    }
    

}
