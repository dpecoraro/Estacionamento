using GreenVille.Domain.DTO;
using GreenVille.Domain.Model;
using System.Threading.Tasks;

namespace GreenVille.Domain.Interfaces.IRepositories
{
    public interface IEstacionamentoRepository : IGenericRepository<Estacionamento>
    {
        Task<Usuario> AutenticarUsuario(string email, string password);

        void RegistrarToken(string token, AuthUserDTO usuario);
    }
}
