using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Model;
using GreenVille.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Repository.Repositories
{
    public class FuncionarioRepository : GenericRepository<Funcionario>, IFuncionarioRepository
    {
        public FuncionarioRepository(DataContext context) : base(context)
        {
        }

        public async Task<Cargo> FindCargo(int cargoId)
        {
            try
            {
                var cargo = await _context.Cargos.FindAsync(cargoId);
                return cargo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<Cargo>> GetCargosAsync()
        {
            try
            {
                var cargos = await _context.Cargos.ToListAsync();
                return cargos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
