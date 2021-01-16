using GreenVille.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services.Interfaces
{
    public interface IVagaApiClient : IBaseApiClient<VagaDTO>
    {
        Task<List<VagaDTO>> GetVagasByEstacionamento(int idEstacionamento);
    }
}
