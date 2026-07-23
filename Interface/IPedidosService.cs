using backend.DTO;
using backend.Model;

namespace backend.Interface;

public interface IPedidosService
{
    Task<Pedidos> PostPedido(Pedidos p);
    
    Task<List<PedidosDTO>> GetAllPedidos();

    Task<Pedidos?> GetPedidoById(int id);

    Task PutPedido(int id);

    Task DeleteMesa(int id);
}