namespace backend.Model
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        
        public virtual ICollection<Produtos> Produtos { get; set; }
    }
}
