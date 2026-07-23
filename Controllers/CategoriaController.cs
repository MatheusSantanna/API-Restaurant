using backend.Interface;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _repository;

    public CategoriaController(ICategoriaService repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Categoria>>> GetCategorias()
    {
        var categorias = await _repository.GetAllCategorias();
        return Ok(categorias);
    }

    [HttpPost]
    public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
    {
        var newCategoria = await _repository.PostCategoria(categoria);
        return Ok(newCategoria);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> GetByCategoria(int id)
    {
        var categoria = await _repository.GetCategoriaById(id);
        return Ok(categoria);
    }
}