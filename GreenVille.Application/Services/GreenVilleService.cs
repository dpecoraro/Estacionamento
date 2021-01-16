using GreenVille.Domain.DTO;
using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Interfaces.IServices;
using System;
using System.Threading.Tasks;

namespace GreenVille.Application.Services
{
    public class GreenVilleService : IGreenVilleService
    {
        private readonly IEstacionamentoRepository _estacionamentoRepository;

        public GreenVilleService(IEstacionamentoRepository estacionamentoRepository)
        {
            _estacionamentoRepository = estacionamentoRepository;
        }

        public async Task<UsuarioDTO> AutenticaUsuario(string email, string password)
        {
            try
            {
                UsuarioDTO usuario = new UsuarioDTO
                {
                    Email = email,
                    Senha = password
                };
                var usuarioAutenticado = await _estacionamentoRepository.AutenticarUsuario(usuario.Email, usuario.Senha);

                if (usuarioAutenticado is null)
                {
                    return null;
                }

                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
