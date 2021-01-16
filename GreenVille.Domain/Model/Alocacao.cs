using System;

namespace GreenVille.Domain.Model
{
    public class Alocacao
    {
        public int Id { get; set; }

        public DateTime Entrada { get; set; }

        public DateTime? Saida { get; set; }

        public float ValorPago { get; set; }

        public bool Mensalista { get; set; }


        public int VagaId { get; set; }
        public Vaga Vaga { get; set; }

        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }

        public int? AtendenteEntradaId { get; set; }
        public Funcionario AtendenteEntrada { get; set; }

        public int? AtendenteSaidaId { get; set; }
        public Funcionario AtendenteSaida { get; set; }

        public int? ManobristaEntradaId { get; set; }
        public Funcionario ManobristaEntrada { get; set; }

        public int? ManobristaSaidaId { get; set; }
        public Funcionario ManobristaSaida { get; set; }

        public double EconomiaCarbono { get; set; }
    }
}
