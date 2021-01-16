using System;

namespace GreenVille.Domain.Model
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string Token { get; set; }

        public DateTime? DataHoraLogin { get; set; }

        public DateTime? DataHoraLogout { get; set; }

        public int? FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }

    }
}
