using backend.DTO;
using backend.Model;

namespace backend.Interface;

public interface IItensPedidoService
{
    Task<ItensPedidoDTO> PostItensPedido(ItensPedidoDTO p);
    
    Task<List<ItensPedido>> GetAllByIdItensPedidos(int it);
 
    Task<List<ItensPedidoDTO>> GetAllCaixaPedido();
    Task<ItensPedido> GetCaixaPedidoById(int id);
}