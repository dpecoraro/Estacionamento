using GreenVille.Domain.DTO;
using System.Threading.Tasks;

namespace GreenVille.Domain.Interfaces.IServices
{
    public interface IGreenVilleService
    {
        Task<UsuarioDTO> AutenticaUsuario(string email, string password);
    }
}
