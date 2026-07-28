using backend.DTO;
using backend.Interface;
using backend.Model;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class TableService : ITableService
    {
        private readonly IRepository<Table> _repository;

        public TableService(IRepository<Table> repository)
        {
            _repository = repository;
        }

        public async Task<List<TableDto>> GetAllTables()
        {
            return await _repository
                .GetAllAsync()
                .OrderBy(table => table.Number)
                .Select(x => new TableDto
                {
                    Number = x.Number,
                    TableStatus = x.TableStatus
                })
                .ToListAsync();
        }

        public async Task<Table> CreateTable(TableDto tableDto)
        {
            var table = new Table
            {
                Number = tableDto.Number,
                TableStatus = TableStatus.Available
            };
            
            await _repository.AddAsync(table);
            return table;
        }

        public async Task<Table?> GetTableById(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteTable(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public void UpdateTable(Table table)
        {
            _repository.Update(table);
        }
    }
}