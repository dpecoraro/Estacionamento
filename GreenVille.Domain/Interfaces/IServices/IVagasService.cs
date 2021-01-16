using GreenVille.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Domain.Interfaces.IServices
{
    public interface IVagasService
    {

        Task<IEnumerable<VagaDTO>> GetVagas();

        Task<VagaDTO> GetVaga(int id);

        Task<VagaDTO> AddVaga(VagaDTO vaga);

        Task<VagaDTO> UpdateVaga(VagaDTO vaga);

        Task<bool> DeleteVaga(int id);

        Task<IEnumerable<VagaDTO>> GetVagasByEstacionamento(int idEstacionamento);

    }
}
