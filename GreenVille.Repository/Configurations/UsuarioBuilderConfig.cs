using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenVille.Repository.Configurations
{
    public class UsuarioBuilderConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Nome)
                .HasColumnName("Nome")
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnName("Email")
                .IsRequired();

            builder.Property(x => x.Senha)
                .HasColumnName("Senha")
                .IsRequired();

            builder.Property(x => x.Token)
                .HasColumnName("Token");

            builder.Property(x => x.DataHoraLogin)
                .HasColumnName("DataHoraLogin");

            builder.Property(x => x.DataHoraLogout)
                .HasColumnName("DataHoraLogout");


            builder.Property(x => x.FuncionarioId)
                .HasColumnName("FuncionarioId");

            builder.HasOne(x => x.Funcionario)
                .WithMany()
                .HasForeignKey(x=>x.FuncionarioId);
        }
    }
}
