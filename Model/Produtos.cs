namespace backend.Model
{
    public class Produtos
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public string Name { get; set; }
        public string Descricao { get; set; }
        public decimal Preco {  get; set; }
        
        public virtual Categoria Categoria { get; set; }
        
    }
}
