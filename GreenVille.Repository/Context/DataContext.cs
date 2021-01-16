using GreenVille.Domain;
using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace GreenVille.Repository.Context
{
    public class DataContext : DbContext
    {
        private readonly Appsettings _settings;

        //private readonly string _connectionString;

        protected DataContext() { }

        public DataContext(DbContextOptions<DataContext> options, Appsettings settings) : base(options)
        {
            _settings = settings;
        }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_settings.ConnectionDetail.DataSource);
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
