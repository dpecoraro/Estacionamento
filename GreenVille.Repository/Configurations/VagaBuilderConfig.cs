using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenVille.Repository.Configurations
{
    public class VagaBuilderConfig : IEntityTypeConfiguration<Vaga>
    {
        public void Configure(EntityTypeBuilder<Vaga> builder)
        {
            builder.ToTable("Vagas");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Nome)
                .HasColumnName("Nome")
                .IsRequired();

            builder.Property(x => x.Interditada)
                .HasColumnName("Interditada")
                .IsRequired();

            builder.Property(x => x.Ocupada)
                .HasColumnName("Ocupada")
                .IsRequired();


            builder.Property(x => x.EstacionamentoId)
                .HasColumnName("EstacionamentoId");

            builder.HasOne(x => x.Estacionamento)
                .WithMany(e => e.Vagas)
                .HasForeignKey(x => x.EstacionamentoId);
        }
    }
}
