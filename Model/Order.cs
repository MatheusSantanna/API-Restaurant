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
        
        public virtual OrderStatus OrderStatus { get; set; } 
    }
    

}
