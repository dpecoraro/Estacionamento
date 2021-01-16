using GreenVille.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services.Interfaces
{
    public interface IFuncionarioApiClient : IBaseApiClient<FuncionarioDTO>
    {
        Task<List<FuncionarioDTO>> GetFuncionariosByEstacionamento(int idEstacionamento);

        Task<List<CargoDTO>> GetAllCargosAsync();

    }
}
