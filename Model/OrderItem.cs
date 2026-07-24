namespace backend.Model
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public double Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public virtual Product Product { get; set; }
    }
}
