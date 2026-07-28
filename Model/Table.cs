namespace backend.Model
{
    public class Table
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public virtual TableStatus  TableStatus { get; set; }
        
    }
    
    
    public enum TableStatus
    {
        Occupied,
        Available,
        Rserved
    }
}
