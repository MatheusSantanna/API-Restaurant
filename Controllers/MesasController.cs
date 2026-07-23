using backend.DTO;
using backend.Interface;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MesasController : ControllerBase
{
    private readonly IMesaService _mesaService;

    public MesasController(IMesaService mesaService)
    {
        _mesaService = mesaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Mesas>>> GetMesas()
    {
        var mesas = await _mesaService.GetAllMesas();
        return Ok(mesas);
    }
    [HttpPost]
    public async Task<ActionResult> PostMesa(Mesas mesa)
    {
       await _mesaService.PostMesa(mesa);
       return Ok(mesa);
    }

    [HttpPut("{id}")]
    public ActionResult<Mesas> PutMesa(int id, [FromBody] Mesas mesa)
    {
        if (id != mesa.Id)
            return BadRequest("O Id informado nao correponde com o da mesa");

        _mesaService.Update(mesa);

       return Ok(mesa);
    }
    
    
}