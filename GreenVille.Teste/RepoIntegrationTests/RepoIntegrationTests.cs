using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace GreenVille.Teste.RepoIntegrationTests
{
    public class RepoIntegrationTests : IClassFixture<DBTestContext>
    {
        private DBTestContext classwideDbContext;

        public RepoIntegrationTests(DBTestContext dbContext)
        {
            classwideDbContext = dbContext;
        }


        [Fact]
        public void TestaBuscaEstacionamento()
        {
            var estacionamentoRegister = classwideDbContext.Estacionamentos.FirstOrDefault();

            Assert.NotNull(estacionamentoRegister);
            Assert.IsType<Estacionamento>(estacionamentoRegister);
            Assert.NotEqual(0, estacionamentoRegister.Id);
        }

        [Fact]
        public void TestaBuscaVagas()
        {
            var listaVagas = classwideDbContext.Vagas.Take(10).ToList();

            Assert.NotNull(listaVagas);
            Assert.True(listaVagas.Count > 0);
        }

        [Fact]
        public void TestaBuscaClienteESeusVeiculos()
        {
            var cliente1 = classwideDbContext.Clientes
                .Include(c => c.VeiculosClientes)
                    .ThenInclude(vc => vc.Veiculo)
                .SingleOrDefault(c => c.Id == 1);

            Assert.NotNull(cliente1);
            Assert.IsType<Cliente>(cliente1);
            Assert.NotNull(cliente1.VeiculosClientes);

            Assert.True(cliente1.VeiculosClientes.Count > 0);
            Assert.NotNull(cliente1.VeiculosClientes.First());

            Assert.True(cliente1.VeiculosClientes.First().ClienteId == cliente1.Id);
        }
    }
}
