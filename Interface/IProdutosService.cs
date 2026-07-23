using backend.Model;
using backend.DTO;


namespace backend.Interface;

public interface IProdutosService
{
    Task<List<Produtos>> GetAllProdutosAsync();
    Task<Produtos> GetProdutoByIdAsync(int id);
    Task<List<ProdutosDTO>> GetProdutosByCategoriaAsync(int id);
    Task DeleteProduto(int id);
    void PutProduto(Produtos p);
    Task PostProduto(ProdutosDTO p);
}