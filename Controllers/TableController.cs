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
    public async Task<ActionResult> CreateTable(Table table)
    {
        await _tableService.CreateTable(table);

        return Ok(table);
    }

    [HttpPut("{id}")]
    public ActionResult<Table> UpdateTable(int id, [FromBody] Table table)
    {
        if (id != table.Id)
            return BadRequest("The provided Id does not match the table Id.");

        _tableService.UpdateTable(table);

        return Ok(table);
    }
}