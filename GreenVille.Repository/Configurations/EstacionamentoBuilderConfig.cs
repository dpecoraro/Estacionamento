using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenVille.Repository.Configurations
{
    public class EstacionamentobuilderuilderConfig : IEntityTypeConfiguration<Estacionamento>
    {
        public void Configure(EntityTypeBuilder<Estacionamento> builder)
        {
            builder.ToTable("Estacionamentos");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.NomeUnidade)
                .HasColumnName("NomeUnidade")
                .IsRequired();

            builder.Property(x => x.ValorHora)
                .HasColumnName("ValorHora")
                .IsRequired();

            builder.Property(x => x.GeracaoCreditosCarbonoHora)
                .HasColumnName("GeracaoCreditosCarbonoHora")
                .IsRequired();
        }
    }
}
