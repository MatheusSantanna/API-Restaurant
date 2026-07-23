using backend.DTO;
using backend.Interface;
using backend.Model;
using backend.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class ItensPedidoService : IItensPedidoService
{
    private readonly IRepository<ItensPedido> _repository;
    private readonly IPedidosService _pedidosService;

    public ItensPedidoService(IRepository<ItensPedido> repository,  IPedidosService pedidosService)
    {
        _repository = repository;
        _pedidosService = pedidosService;
    }

    public async Task<ItensPedidoDTO> PostItensPedido(ItensPedidoDTO dto)
    {
        var item = new ItensPedido
        {
            PedidoId = dto.PedidoId,
            ProdutosId = dto.ProdutosId,
            Quantidade = dto.Quantidade,
            PrecoUnitario = dto.PrecoUnitario
        };
        await _repository.AddAsync(item);

        await _pedidosService.PutPedido(item.PedidoId);
        
        return dto;
    }

    public async Task<List<ItensPedidoDTO>> GetAllCaixaPedido()
    {
        return await _repository.GetAllAsync()
            .OrderBy(x => x.Id)
            .ThenBy(x => x.PedidoId)
            .Select(x => new ItensPedidoDTO
            {
                PedidoId = x.PedidoId,
                ProdutosId = x.ProdutosId,
                Quantidade = x.Quantidade,
                PrecoUnitario = x.PrecoUnitario
            }).ToListAsync();
    }

    public async Task<ItensPedido?> GetCaixaPedidoById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<ItensPedido>> GetAllByIdItensPedidos(int id)
    {
        return await _repository
            .GetAllAsync()
            .Where(x => x.PedidoId == id)
            .ToListAsync();;
    }
    
}