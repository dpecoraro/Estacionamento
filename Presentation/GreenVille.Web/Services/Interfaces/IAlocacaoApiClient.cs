using GreenVille.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services.Interfaces
{
    public interface IAlocacaoApiClient : IBaseApiClient<AlocacaoDTO>
    {
        Task<List<VeiculoAlocadoDTO>> GetAlocacoesPorEstacionamento(int idEstacionamento);

        Task<AlocacaoDTO> GetAlocacaoPorPlaca(string plateNumber);
    }
}