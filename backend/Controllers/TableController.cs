using backend.DTO;
using backend.Interface;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TableController : ControllerBase
{
    private readonly ITableService _tableService;

    public TableController(ITableService tableService)
    {
        _tableService = tableService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Table>>> GetTables()
    {
        var tables = await _tableService.GetAllTables();

        return Ok(tables);
    }

    [HttpPost]
    public async Task<ActionResult> CreateTable(TableDto table)
    {
        await _tableService.CreateTable(table);

        return Ok(table);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TableDto>> GetByIdTable(int id)
    {
        var table = await _tableService.GetTableById(id);
        if (table == null)
            return NotFound();
        return Ok(table);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Table>> UpdateTable(int id, [FromBody] TableStatus status)
    {
        // 1. Adicione o AWAIT aqui!
        var updatedTable = await _tableService.UpdateStatusTable(id, status);

        if (updatedTable == null)
        {
            return NotFound($"Mesa {id} não encontrada.");
        }

        // 2. Retorna a mesa resolvida
        return Ok(updatedTable);
    }
}