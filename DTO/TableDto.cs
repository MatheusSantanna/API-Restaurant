using backend.Model;

namespace backend.DTO
{
    public class TableDto
    {
        public int Number { get; set; }

        public virtual TableStatus  TableStatus { get; set; }
    }
}