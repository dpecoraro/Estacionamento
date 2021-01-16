using System.ComponentModel.DataAnnotations;

namespace GreenVille.Domain.DTO
{
    public class AuthUserDTO
    {
        [Required(ErrorMessage = "E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O E-mail não está em um formato válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória.")]
        public string Password { get; set; }
    }
}