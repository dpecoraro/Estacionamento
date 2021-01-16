using System.ComponentModel.DataAnnotations;

namespace GreenVille.Domain.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage ="O E-mail não está em um formato válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Confirmação de E-mail é obrigatório")]
        [Compare("Email", ErrorMessage = "A confirmação de E-mail precisar igual ao e-mail")]
        public string EmailConfirmation { get; set; }


        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(8, ErrorMessage = "A senha precisa ter no mínimo 8 caracteres")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Confirmação de Senha é obrigatória")]
        [Compare("Senha", ErrorMessage = "A confirmação de Senha precisar igual à Senha")]
        public string SenhaConfirmation { get; set; }


        public bool FuncionarioVinculado { get; set; }
        public int FuncionarioId { get; set; }
        public string FuncionarioNome { get; set; }
        public string FuncionarioCPF { get; set; }

    }
}
