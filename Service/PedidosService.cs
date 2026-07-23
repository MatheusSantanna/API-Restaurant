using backend.DTO;
using backend.Interface;
using backend.Model;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class PedidosService : IPedidosService
{
    private readonly IRepository<Pedidos> _pedidosRepository;
    private readonly IMesaService _mesaService;
    private readonly IItensPedidoService _itensPedidoService;

    public PedidosService(IRepository<Pedidos> pedidosRepository, IMesaService mesaService,  IItensPedidoService itensPedidoService)
    {
        _pedidosRepository = pedidosRepository;
        _mesaService = mesaService;
        _itensPedidoService = itensPedidoService;
    }
    
    
    public async Task<Pedidos> PostPedido(Pedidos p)
    {
        await _pedidosRepository.AddAsync(p);
        return p;
    }

    public async Task<List<PedidosDTO>> GetAllPedidos()
    {
        
        return await _pedidosRepository
            .GetAllAsync()
            .OrderBy(x => x.DataPedido)
            .ThenBy(x => x.StatusPedido)
            .Select(x => new PedidosDTO
            {
                MesaId = x.MesaId,
                DataPedido = x.DataPedido,
                StatusPedido = x.StatusPedido,
                ValorTotal = x.ValorTotal,
            })
            .ToListAsync();
    }

    public async Task<Pedidos?> GetPedidoById(int id)
    {
        return await _pedidosRepository.GetByIdAsync(id);
    }

    public async Task DeleteMesa(int id)
    {
        await  _pedidosRepository.DeleteAsync(id);
    }
    
    public async Task PutPedido(int id)
    {
        var pedido = await GetPedidoById(id);

        var itens = await _itensPedidoService.GetAllByIdItensPedidos(id);

        pedido.ValorTotal = itens.Sum(x => (decimal)x.Quantidade * x.PrecoUnitario);
       
        _pedidosRepository.Update(pedido);
    }
}