using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Model;
using GreenVille.Repository.Context;

namespace GreenVille.Repository.Repositories
{
    public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(DataContext context) : base(context)
        {

        }
    }
}
