using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenVille.Domain.DTO
{
    public class ClienteDTO
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; }


        [Required(ErrorMessage = "* Campo obrigatório")]
        [MaxLength(15, ErrorMessage = "Telefone deve conter no máximo 15 caracteres (Incluindo parentêses, espaço e traço)")]
        [RegularExpression(@"^\([1-9]{2}\) (?:[2-8]|9[1-9])[0-9]{3}\-[0-9]{4}$", ErrorMessage = "Telefone deve estar no formato (00) 00000-0000 (Incluindo parentêses e traço)")]
        public string Telefone { get; set; }


        [Required(ErrorMessage = "* Campo obrigatório")]
        [MaxLength(14, ErrorMessage = "CPF deve conter no máximo 14 caracteres (Incluindo pontos e traço)")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$", ErrorMessage = "CPF deve estar no formato 000.000.000-00 (Incluindo pontos e traço)")]
        public string CPF { get; set; }


        public ICollection<VeiculoClienteDTO> VeiculosClientes { get; set; }

    }
}
