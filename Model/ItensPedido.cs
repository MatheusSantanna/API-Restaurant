namespace backend.Model
{
    public class ItensPedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProdutosId { get; set; }
        public double Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        
        public virtual Produtos Produtos { get; set; }
    }
}
