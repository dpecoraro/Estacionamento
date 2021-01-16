using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenVille.Repository.Configurations
{
    public class FuncionarioBuilderConfig : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> builder)
        {
            builder.ToTable("Funcionarios");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CPF)
                .HasColumnName("CPF")
                .HasMaxLength(14)
                .IsRequired();

            builder.Property(x => x.Nome)
                .HasColumnName("Nome")
                .IsRequired();


            builder.Property(x => x.CargoId)
                .HasColumnName("CargoId")
                .IsRequired();

            builder.HasOne(x => x.Cargo)
                .WithMany()
                .HasForeignKey(x => x.CargoId)
                .IsRequired();


            builder.Property(x => x.EstacionamentoId)
                .HasColumnName("EstacionamentoId")
                .IsRequired();

            builder.HasOne(x => x.Estacionamento)
                .WithMany()
                .HasForeignKey(x => x.EstacionamentoId);
        }
    }
}
