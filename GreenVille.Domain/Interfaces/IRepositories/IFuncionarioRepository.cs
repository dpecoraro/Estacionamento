using GreenVille.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Domain.Interfaces.IRepositories
{
    public interface IFuncionarioRepository : IGenericRepository<Funcionario>
    {
        Task<Cargo> FindCargo(int cargoId);

        Task<IEnumerable<Cargo>> GetCargosAsync();
    }
}
