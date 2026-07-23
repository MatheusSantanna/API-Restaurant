using backend.DTO;
using backend.Interface;
using backend.Model;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class MesaService : IMesaService
    {
        private readonly IRepository<Mesas> _repository;

        public MesaService(IRepository<Mesas> repository)
        {
            _repository = repository;
        }

        public async Task<List<MesasDTO>> GetAllMesas() 
        {
            return await _repository
                .GetAllAsync()
                .OrderBy(nMesa => nMesa.Numero)
                .Select(x => new MesasDTO
                {
                    Id = x.Id,
                    Numero = x.Numero,
                    Status = x.Status,
                })
                .ToListAsync();                           
        }

        public async Task<Mesas> PostMesa(Mesas m)
        {
            await _repository.AddAsync(m);
            return m;
        }


        public async Task<Mesas?> GetMesaById(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteMesa(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public void Update(Mesas mesa)
        {
            _repository.Update(mesa);
            
        }
    }
}
