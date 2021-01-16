using GreenVille.Application.Exceptions;
using GreenVille.Domain.DTO;
using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Interfaces.IServices;
using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Application.Services
{
    public class UsuariosService : IUsuariosService
    {
        private const string ERR_MSG_USUARIO_NOT_FOUND = "Usuário de Id {0} não encontrado.";
        private const string ERR_MSG_FUNCIONARIO_PARA_USUARIO_NOT_FOUND = "Funcionário selecionado para o Usuário não existente.";
        private const string ERR_MSG_USUARIO_ALREADY_EXISTS = "Usuário de Id {0} já existe na base de dados.";

        private readonly IUsuarioRepository _baseRepo;
        private readonly IFuncionarioRepository _funcionarioRepository;

        public UsuariosService(IUsuarioRepository usuarioRepository, IFuncionarioRepository funcionarioRepository)
        {
            _baseRepo = usuarioRepository;
            _funcionarioRepository = funcionarioRepository;
        }


        public async Task<UsuarioDTO> AddUsuario(UsuarioDTO usuario)
        {
            try
            {
                //checks if entry already exists
                var usuarioRegisterExists = await UsuarioExists(usuario.Id);
                if (usuarioRegisterExists)
                {
                    throw new RegisterDuplicatedException(string.Format(ERR_MSG_USUARIO_ALREADY_EXISTS, usuario.Id));
                }

                //retrives Funcionario
                if (usuario.FuncionarioVinculado)
                {
                    var funcionario = await _funcionarioRepository.GetAsync(usuario.FuncionarioId);
                    if (null == funcionario) { throw new RegisterNotFoundException(ERR_MSG_FUNCIONARIO_PARA_USUARIO_NOT_FOUND); }
                }

                var usuarioRegister = new Usuario
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Senha = usuario.Senha,
                    FuncionarioId = usuario.FuncionarioVinculado ? usuario.FuncionarioId : (int?)null
                };

                await _baseRepo.AddAsync(usuarioRegister);


                //salve all changes
                await _baseRepo.SaveAllAsync();

                usuario.Id = usuarioRegister.Id;
                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<UsuarioDTO> UpdateUsuario(UsuarioDTO usuario)
        {
            try
            {
                //retrives Funcionario
                if (usuario.FuncionarioVinculado)
                {
                    var funcionario = await _funcionarioRepository.GetAsync(usuario.FuncionarioId);
                    if (null == funcionario) { throw new RegisterNotFoundException(ERR_MSG_FUNCIONARIO_PARA_USUARIO_NOT_FOUND); }
                }


                //checks if usuario already exists
                var usuarioRegister = await _baseRepo.GetAsync(usuario.Id);
                if (null == usuarioRegister)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_USUARIO_NOT_FOUND, usuario.Id));
                }
                else
                {
                    usuarioRegister.Nome = usuario.Nome;
                    usuarioRegister.Email = usuario.Email;
                    usuarioRegister.Senha = usuario.Senha;
                    usuarioRegister.FuncionarioId = usuario.FuncionarioVinculado ? usuario.FuncionarioId : (int?)null;

                    _baseRepo.Update(usuarioRegister);
                }

                //salve all changes
                await _baseRepo.SaveAllAsync();

                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> DeleteUsuario(int id)
        {
            try
            {
                //checks if entry already exists
                var usuarioRegisterExists = await UsuarioExists(id);
                if (!usuarioRegisterExists)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_USUARIO_NOT_FOUND, id));
                }

                //deletes the entry
                _baseRepo.Delete(x => x.Id == id);

                //salve all changes
                await _baseRepo.SaveAllAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<UsuarioDTO> GetUsuario(int id)
        {
            try
            {
                var search = _baseRepo.QueryAll()
                                        .Include(f => f.Funcionario);

                var usuario = await search.SingleOrDefaultAsync(x => x.Id == id);

                if (null == usuario)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_USUARIO_NOT_FOUND, id));
                }

                var usuarioDTO = new UsuarioDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Senha = usuario.Senha,
                    FuncionarioVinculado = usuario.FuncionarioId.HasValue,
                    FuncionarioId = (usuario.FuncionarioId.HasValue ? usuario.FuncionarioId.Value : 0),
                    FuncionarioNome = usuario.Funcionario?.Nome,
                    FuncionarioCPF = usuario.Funcionario?.CPF
                };

                return usuarioDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<UsuarioDTO>> GetUsuarios()
        {
            try
            {
                var usuarios = await _baseRepo.QueryAll()
                                                    .Include(f => f.Funcionario)
                                                    .ToListAsync();

                var usuariosDTO = new List<UsuarioDTO>();
                foreach (var usuario in usuarios)
                {
                    var usuarioDTO = new UsuarioDTO
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Email = usuario.Email,
                        Senha = usuario.Senha,
                        FuncionarioVinculado = usuario.FuncionarioId.HasValue,
                        FuncionarioId = (usuario.FuncionarioId.HasValue ? usuario.FuncionarioId.Value : 0),
                        FuncionarioNome = usuario.Funcionario?.Nome,
                        FuncionarioCPF = usuario.Funcionario?.CPF
                    };
                    usuariosDTO.Add(usuarioDTO);
                }

                return usuariosDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<UsuarioDTO> GetUsuarioByMail(string userEmail)
        {
            try
            {
                var usuario = await _baseRepo.QueryAll().FirstOrDefaultAsync(x => x.Email == userEmail);

                if (null == usuario)
                {
                    return null;
                }

                var usuarioDTO = new UsuarioDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Senha = usuario.Senha
                };

                return usuarioDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async Task<bool> UsuarioExists(int id)
        {
            return await _baseRepo.Exists(id);
        }

    }
}
