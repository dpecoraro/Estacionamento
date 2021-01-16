using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenVille.Repository.Configurations
{
    public class ClienteBuilderConfig : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CPF)
                    .IsRequired()
                    .HasColumnName("CPF")
                    .HasMaxLength(14);

            builder.Property(x => x.Nome)
                    .IsRequired()
                    .HasColumnName("Nome");

            builder.Property(x => x.Telefone)
                    .HasColumnName("Telefone");
        }
    }
}
