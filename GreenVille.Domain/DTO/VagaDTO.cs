using System.ComponentModel.DataAnnotations;

namespace GreenVille.Domain.DTO
{
    public class VagaDTO
    {

        public int Id { get; set; }


        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; }

        public bool Ocupada { get; set; }

        public bool Interditada { get; set; }



        [Required(ErrorMessage = "É obrigatório selecionar-se um Estacionamento")]
        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar-se um Estacionamento")]
        public int EstacionamentoId { get; set; }
        public string EstacionamentoUnidade { get; set; }

    }
}
