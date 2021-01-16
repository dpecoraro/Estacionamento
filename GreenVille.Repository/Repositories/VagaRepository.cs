using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Model;
using GreenVille.Repository.Context;

namespace GreenVille.Repository.Repositories
{
    public class VagaRepository : GenericRepository<Vaga>, IVagaRepository
    {
        public VagaRepository(DataContext context) : base(context)
        {

        }
    }
}
