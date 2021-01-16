using GreenVille.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Domain.Interfaces.IServices
{
    public interface IEstacionamentosService
    {

        Task<IEnumerable<EstacionamentoDTO>> GetEstacionamentos();

        Task<EstacionamentoDTO> GetEstacionamento(int id);

        Task<EstacionamentoDTO> AddEstacionamento(EstacionamentoDTO estacionamento);

        Task<EstacionamentoDTO> UpdateEstacionamento(EstacionamentoDTO estacionamento);

        Task<bool> DeleteEstacionamento(int id);

    }
}
