using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Model;
using GreenVille.Repository.Context;

namespace GreenVille.Repository.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(DataContext context) : base(context)
        {

        }
    }
}
