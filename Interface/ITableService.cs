using backend.DTO;
using backend.Model;

namespace backend.Interface
{
    public interface ITableService
    {
        Task<Table> CreateTable(TableDto table);

        Task<List<TableDto>> GetAllTables();

        Task<Table?> GetTableById(int id);

        void UpdateTable(Table table);

        Task DeleteTable(int id);
    }
}