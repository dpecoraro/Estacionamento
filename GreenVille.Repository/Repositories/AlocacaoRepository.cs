using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Model;
using GreenVille.Repository.Context;

namespace GreenVille.Repository.Repositories
{
    public class AlocacaoRepository : GenericRepository<Alocacao>, IAlocacaoRepository
    {
        public AlocacaoRepository(DataContext context) : base(context)
        {

        }
    }
}
