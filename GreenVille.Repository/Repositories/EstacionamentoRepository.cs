using GreenVille.Domain.DTO;
using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Model;
using GreenVille.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenVille.Repository.Repositories
{
    public class EstacionamentoRepository : GenericRepository<Estacionamento>, IEstacionamentoRepository
    {
        public EstacionamentoRepository(DataContext context) : base(context)
        {

        }

        public async Task<Usuario> AutenticarUsuario(string email, string password)
        {
            var usuario = (from user in _context.Usuarios
                           join funcionario in _context.Funcionarios
                           on user.Funcionario.Id equals funcionario.Id
                           where user.Email == email && user.Senha == password
                           select new Usuario
                           {
                               Id = user.Id,
                               Email = user.Email,
                               Senha = user.Senha,
                               Funcionario = new Funcionario
                               {
                                   Id = funcionario.Id,
                                   Nome = funcionario.Nome,
                                   Cargo = funcionario.Cargo,
                                   CPF = funcionario.CPF
                               }
                           }
                          ).FirstOrDefaultAsync();

            return await usuario;

        }

        public void RegistrarToken(string token, AuthUserDTO usuario)
        {
            try
            {
                Usuario usuarioAuth = new Usuario
                {
                    Email = usuario.Email,
                    DataHoraLogin = DateTime.Now,
                    Token = token
                };

                _context.Usuarios.Update(usuarioAuth);
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
