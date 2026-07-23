using backend.Interface;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidosService _pedidosService;

    public PedidosController(IPedidosService pedidosService)
    {
        _pedidosService = pedidosService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await _pedidosService.GetAllPedidos();
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        await _pedidosService.GetPedidoById(id);
        return Ok(id);
    }

    [HttpPost]
    public async Task<IActionResult> PostPedidos(Pedidos pedido)
    {
        await _pedidosService.PostPedido(pedido);
        return Ok(pedido);
    }

    [HttpPut]
    public async Task<IActionResult> PutPedidos(int idPedido)
    {
        await _pedidosService.PutPedido(idPedido);
        return Ok(idPedido);
    }
    
}