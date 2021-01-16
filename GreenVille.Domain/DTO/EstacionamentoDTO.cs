using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenVille.Domain.DTO
{
    public class EstacionamentoDTO
    {

        public int Id { get; set; }


        [Required(ErrorMessage = "Nome da Unidade é obrigatório")]
        public string NomeUnidade { get; set; }


        [Required(ErrorMessage = "Valor da Hora é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor da Hora precisa ser positivo")]
        public double ValorHora { get; set; }


        [Required(ErrorMessage = "Média Estimada de Economia de Carbono é necessária")]
        [Range(0, double.MaxValue, ErrorMessage = "Média Estimada de Economia de Carbono ser positivo")]
        public double GeracaoCreditosCarbonoHora { get; set; }


        public ICollection<VagaDTO> Vagas { get; set; }

    }
}
