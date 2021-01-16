using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenVille.Repository.Configurations
{
    public class VeiculoBuilderConfig : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.ToTable("Veiculos");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Placa)
                .HasColumnName("Placa")
                .IsRequired();

            builder.Property(x => x.Modelo)
                .HasColumnName("Modelo");

            builder.Property(x => x.Ano)
                .HasColumnName("Ano")
                .HasMaxLength(4);

            builder.Property(x => x.Cor)
                .HasColumnName("Cor");

            builder.Property(x => x.Mensalista)
                .HasColumnName("Mensalista");
        }
    }
}
