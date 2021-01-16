using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Model;
using GreenVille.Repository.Context;

namespace GreenVille.Repository.Repositories
{
    public class VeiculoRepository : GenericRepository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(DataContext context) : base(context)
        {

        }
    }
}
