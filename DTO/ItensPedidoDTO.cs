namespace backend.DTO;

public class ItensPedidoDTO
{
    public int PedidoId { get; set; }
    public int ProdutosId { get; set; }
    public double Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}