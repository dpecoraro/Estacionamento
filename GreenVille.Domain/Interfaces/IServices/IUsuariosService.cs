using GreenVille.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Domain.Interfaces.IServices
{
    public interface IUsuariosService
    {

        Task<IEnumerable<UsuarioDTO>> GetUsuarios();

        Task<UsuarioDTO> GetUsuario(int id);

        Task<UsuarioDTO> AddUsuario(UsuarioDTO funcionario);

        Task<UsuarioDTO> UpdateUsuario(UsuarioDTO funcionario);

        Task<bool> DeleteUsuario(int id);

        Task<UsuarioDTO> GetUsuarioByMail(string userEmail);

    }
}
