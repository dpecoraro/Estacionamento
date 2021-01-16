using GreenVille.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services.Interfaces
{
    public interface IVeiculoApiClient : IBaseApiClient<VeiculoDTO>
    {
        Task<VeiculoDTO> GetVeiculoPorPlaca(string placa);

        Task<List<VeiculoDTO>> GetVeiculosNaoEstacionados();
    }
}