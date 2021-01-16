using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenVille.Repository.Configurations
{
    public class VeiculoClienteBuildConfig : IEntityTypeConfiguration<VeiculoCliente>
    {
        public void Configure(EntityTypeBuilder<VeiculoCliente> builder)
        {
            builder.ToTable("VeiculosClientes");

            builder.HasKey(vc => new { vc.ClienteId, vc.VeiculoId });
            builder.HasIndex(vc => new { vc.ClienteId, vc.VeiculoId });

            builder.Property(vc => vc.ClienteId)
                .HasColumnName("ClienteId");

            builder.Property(vc => vc.VeiculoId)
                .HasColumnName("VeiculoId");

            builder.HasOne(vc => vc.Veiculo)
                .WithMany(c => c.VeiculosClientes)
                .HasForeignKey(vc => vc.VeiculoId);

            builder.HasOne(vc => vc.Cliente)
                .WithMany(v => v.VeiculosClientes)
                .HasForeignKey(vc => vc.ClienteId);
        }
    }
}
