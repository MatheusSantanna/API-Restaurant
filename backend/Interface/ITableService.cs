using backend.DTO;
using backend.Model;

namespace backend.Interface
{
    public interface ITableService
    {
        Task<Table> CreateTable(TableDto table);

        Task<List<TableDto>> GetAllTables();

        Task<TableDto?> GetTableById(int id);
        

        Task<Table> UpdateStatusTable(int Idtable, TableStatus status);

        Task DeleteTable(int id);
    }
}