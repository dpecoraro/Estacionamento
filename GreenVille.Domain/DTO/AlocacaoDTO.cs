using System;
using System.ComponentModel.DataAnnotations;

namespace GreenVille.Domain.DTO
{
    public class AlocacaoDTO
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Data/Hora de entrada é obrigatório")]
        public DateTime Entrada { get; set; }

        public DateTime? Saida { get; set; }


        [Required(ErrorMessage = "Valor Pago é obrigatório")]
        public float ValorPago { get; set; }

        public bool Mensalista { get; set; }


        [Required(ErrorMessage = "É obrigatório selecionar-se uma Vaga")]
        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar-se uma Vaga")]
        public int VagaId { get; set; }
        public string VagaNome { get; set; }
        public int EstacionamentoId { get; set; }
        public string EstacionamentoUnidade { get; set; }


        [Required(ErrorMessage = "É obrigatório selecionar-se um Veículo")]
        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar-se um Veículo")]
        public int VeiculoId { get; set; }
        public string VeiculoPlaca { get; set; }



        [Required(ErrorMessage = "É obrigatório selecionar-se um Atendente de Entrada")]
        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar-se um Atendente de Entrada")]
        public int AtendenteEntradaId { get; set; }
        public string AtendenteEntradaCPF { get; set; }
        public string AtendenteEntradaNome { get; set; }

        public int AtendenteSaidaId { get; set; }
        public string AtendenteSaidaCPF { get; set; }
        public string AtendenteSaidaNome { get; set; }



        [Required(ErrorMessage = "É obrigatório selecionar-se um Manobrista de Entrada")]
        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar-se um Manobrista de Entrada")]
        public int ManobristaEntradaId { get; set; }
        public string ManobristaEntradaCPF { get; set; }
        public string ManobristaEntradaNome { get; set; }

        public int ManobristaSaidaId { get; set; }
        public string ManobristaSaidaCPF { get; set; }
        public string ManobristaSaidaNome { get; set; }


        [Range(0, double.MaxValue, ErrorMessage = "A quantidade de economia de carbono não pode ser negativa")]
        public double EconomiaCarbono { get; set; }
    }
}
