namespace backend.Model
{
    public class Pedidos
    {
        public int Id { get; set; }
        public int MesaId { get; set; }
        public DateTime DataPedido { get; set; }
        public string StatusPedido { get; set; }
        public decimal ValorTotal { get; set; }

    }
}
