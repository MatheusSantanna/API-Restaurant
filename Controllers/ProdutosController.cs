using backend.DTO;
using backend.Interface;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutosService _repository;

    public ProdutosController(IProdutosService repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var produtos = await _repository.GetAllProdutosAsync();
        return Ok(produtos);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProdutosDTO produtos)
    {
        await _repository.PostProduto(produtos);
        return Ok(produtos);
    }
    
    [HttpPut]
    public IActionResult Put(int id, [FromBody] Produtos produtos)
    {
        var produto = _repository.GetProdutoByIdAsync(id);
        if (produto.Id == id)
        {
            _repository.PutProduto(produtos);
            return Ok(produtos);
        }
        else{
            return BadRequest();
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByCategoria(int id)
    {
        var categoria = await _repository.GetProdutoByIdAsync(id);
        return Ok(categoria);
    }
}