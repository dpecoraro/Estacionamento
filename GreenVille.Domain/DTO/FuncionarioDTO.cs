using System.ComponentModel.DataAnnotations;

namespace GreenVille.Domain.DTO
{
    public class FuncionarioDTO
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        [MaxLength(14, ErrorMessage = "CPF deve conter no máximo 14 caracteres (incluindo pontos e traço)")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$", ErrorMessage = "CPF deve estar no formato 000.000.000-00 (incluindo pontos e traço)")]
        public string CPF { get; set; }


        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; }


        [Required(ErrorMessage = "É obrigatório selecionar-se um Cargo")]
        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar-se um Cargo")]
        public int CargoId { get; set; }
        public string CargoDescricao { get; set; }


        [Required(ErrorMessage = "É obrigatório selecionar-se um Estacionamento")]
        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar-se um Estacionamento")]
        public int EstacionamentoId { get; set; }
        public string EstacionamentoUnidade { get; set; }

    }
}
