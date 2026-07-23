using backend.DTO;
using backend.Interface;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItensPedidoController : ControllerBase
{
    private readonly IItensPedidoService _repositoryItens;

    public ItensPedidoController(IItensPedidoService repositoryItens)
    {
        _repositoryItens = repositoryItens;
    }

    [HttpGet]
    public async Task<IActionResult> GetItensPedido()
    {
        var itensPedido = await _repositoryItens.GetAllCaixaPedido();
        return Ok(itensPedido);
    }
    [HttpPost]
    public async Task<IActionResult> PostItensPedido([FromBody] ItensPedidoDTO item)
    {
        await _repositoryItens.PostItensPedido(item);
        return Ok(item);
    }
    
}