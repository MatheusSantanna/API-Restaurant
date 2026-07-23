using backend.DTO;
using backend.Interface;
using backend.Model;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;


namespace backend.Service;

public class ProdutosService : IProdutosService
{
    private readonly IRepository<Produtos> _repositoryProd;
    private readonly ICategoriaService _repositoryCategoria;
   
    
    public ProdutosService(IRepository<Produtos> repositoryProd, ICategoriaService repositoryCategoria)
    {
        _repositoryProd = repositoryProd;
        _repositoryCategoria = repositoryCategoria;
      
    }

    public async Task PostProduto(ProdutosDTO p)
    {
        var mProduto = new Produtos
        {
            CategoriaId = p.CategoriaId,
            Name = p.Name,
            Descricao = p.Descricao,
            Preco = p.Preco
        };
        await _repositoryProd.AddAsync(mProduto);
    }

    public async Task<Produtos?> GetProdutoByIdAsync(int id)
    {
        var produto = await _repositoryProd.GetByIdAsync(id);
        return produto;
    }

    public async Task<List<Produtos>> GetAllProdutosAsync()
    {
        return await _repositoryProd.GetAllAsync()
            .OrderBy(x => x.Id)
            .ThenBy(x => x.CategoriaId)
            .Select(x => new Produtos
            {
                CategoriaId = x.CategoriaId,
                Name = x.Name,
                Descricao = x.Descricao,
                Preco = x.Preco
            }).ToListAsync();
    }

    public async Task<List<ProdutosDTO>> GetProdutosByCategoriaAsync(int id)
    {
        var produtos = _repositoryProd.GetAllAsync();
        var produtosByCat = await
            produtos
                .Where(x => x.CategoriaId == id)
                .Select(x => new ProdutosDTO
                {
                    CategoriaId = x.CategoriaId,
                    Name = x.Name,
                    Descricao = x.Descricao,
                    Preco = x.Preco,

                }).ToListAsync();

        return produtosByCat;
    }

    public void PutProduto(Produtos p)
    {
        _repositoryProd.Update(p);
    }

    public async Task DeleteProduto(int id)

    {
         await _repositoryProd.DeleteAsync(id);
    }
}