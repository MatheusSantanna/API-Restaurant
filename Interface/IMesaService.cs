using backend.DTO;
using backend.Model;

namespace backend.Interface
{
    public interface IMesaService 
    {
        Task<Mesas> PostMesa(Mesas mesa);

        Task<List<MesasDTO>> GetAllMesas();

        Task<Mesas?> GetMesaById(int id);

        void Update(Mesas mesa);

        Task DeleteMesa(int id);

    }
}
