using System.Text.Json.Serialization;
using backend.Model;

namespace backend.DTO
{
    public class TableDto
    {
        public int TableId { get; set; }
        public int Number { get; set; }
        public virtual TableStatus  TableStatus { get; set; }
    }
}