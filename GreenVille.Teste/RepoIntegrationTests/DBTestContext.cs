using GreenVille.Domain.Model;
using GreenVille.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace GreenVille.Teste.RepoIntegrationTests
{
    public class DBTestContext : DbContext, IDisposable
    {
        private string connString = "Data Source=greenvilleserver.database.windows.net,1433;Initial Catalog=greenvilleDB;User ID=greenvilleAdmin;Password=893jwan9_;";


        /// <summary>
        /// Setup the database for the integration test
        /// </summary>
        public DBTestContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        }



        #region [ DB Sets ]

        public virtual DbSet<Cliente> Clientes { get; set; }

        public virtual DbSet<Veiculo> Veiculos { get; set; }

        public virtual DbSet<Alocacao> Alocacoes { get; set; }

        public virtual DbSet<Cargo> Cargos { get; set; }

        public virtual DbSet<Estacionamento> Estacionamentos { get; set; }

        public virtual DbSet<Funcionario> Funcionarios { get; set; }

        public virtual DbSet<Vaga> Vagas { get; set; }

        public virtual DbSet<VeiculoCliente> VeiculosClientes { get; set; }

        public virtual DbSet<Usuario> Usuarios { get; set; }

        #endregion
    }
}
