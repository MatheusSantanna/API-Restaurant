using backend.Interface;
using backend.Model;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class CategoriaService : ICategoriaService
{
    private readonly IRepository<Categoria> _repository;
    
    public CategoriaService(IRepository<Categoria> repository)
    {
        _repository = repository;
    }

    public async Task<Categoria> PostCategoria(Categoria categoria)
    {
        await   _repository.AddAsync(categoria);
        return categoria;
            
    }

    public async Task<List<Categoria>> GetAllCategorias()
    {
        return await _repository.GetAllAsync().ToListAsync();
    }

    public async Task<Categoria?> GetCategoriaById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}