using GreenVille.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Domain.Interfaces.IServices
{
    public interface IFuncionariosService
    {

        Task<IEnumerable<FuncionarioDTO>> GetFuncionarios(int? estacionamentoId = null);

        Task<FuncionarioDTO> GetFuncionario(int id);

        Task<FuncionarioDTO> AddFuncionario(FuncionarioDTO funcionario);

        Task<FuncionarioDTO> UpdateFuncionario(FuncionarioDTO funcionario);

        Task<bool> DeleteFuncionario(int id);


        Task<IEnumerable<CargoDTO>> GetCargos();

    }
}
