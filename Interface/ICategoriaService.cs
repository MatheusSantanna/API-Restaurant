using backend.Model;

namespace backend.Interface;

public interface ICategoriaService
{
    Task<List<Categoria>> GetAllCategorias();
    
    Task<Categoria> GetCategoriaById(int id);
    
    Task<Categoria> PostCategoria(Categoria categoria);
}