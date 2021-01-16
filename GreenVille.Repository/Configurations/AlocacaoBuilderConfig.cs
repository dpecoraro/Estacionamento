using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenVille.Repository.Configurations
{
    public class AlocacaoBuilderConfig : IEntityTypeConfiguration<Alocacao>
    {
        public void Configure(EntityTypeBuilder<Alocacao> builder)
        {
            builder.ToTable("Alocacoes");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Entrada)
                .HasColumnName("MomentoEntrada")
                .IsRequired();

            builder.Property(x => x.Saida)
                .HasColumnName("MomentoSaida");

            builder.Property(x => x.Mensalista)
                .HasColumnName("Mensalista")
                .IsRequired();

            builder.Property(x => x.ValorPago)
                .HasColumnName("ValorPago");


            builder.Property(x => x.ManobristaEntradaId)
                .HasColumnName("ManobristaEntradaId");

            builder.HasOne(x => x.ManobristaEntrada)
                .WithMany()
                .HasForeignKey(x => x.ManobristaEntradaId);


            builder.Property(x => x.ManobristaSaidaId)
                .HasColumnName("ManobristaSaidaId");

            builder.HasOne(x => x.ManobristaSaida)
                .WithMany()
                .HasForeignKey(x => x.ManobristaSaidaId);


            builder.Property(x => x.AtendenteEntradaId)
                .HasColumnName("AtendenteEntradaId");

            builder.HasOne(x => x.AtendenteEntrada)
                .WithMany()
                .HasForeignKey(x => x.AtendenteEntradaId);


            builder.Property(x => x.AtendenteSaidaId)
                .HasColumnName("AtendenteSaidaId");

            builder.HasOne(x => x.AtendenteSaida)
                .WithMany()
                .HasForeignKey(x => x.AtendenteSaidaId);


            builder.Property(x => x.VeiculoId)
                .HasColumnName("VeiculoId")
                .IsRequired();

            builder.HasOne(x => x.Veiculo)
                .WithMany()
                .HasForeignKey(x => x.VeiculoId);


            builder.Property(x => x.VagaId)
                .HasColumnName("VagaId")
                .IsRequired();

            builder.HasOne(x => x.Vaga)
                .WithMany()
                .HasForeignKey(x => x.VagaId);


            builder.Property(x => x.EconomiaCarbono)
                .HasColumnName("EconomiaCarbono");

        }
    }
}
