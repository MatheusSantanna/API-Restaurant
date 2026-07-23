namespace backend.DTO;

public class PedidosDTO
{
    public int MesaId { get; set; }
    public DateTime DataPedido { get; set; }
    public string StatusPedido { get; set; }
    public decimal ValorTotal { get; set; }
}