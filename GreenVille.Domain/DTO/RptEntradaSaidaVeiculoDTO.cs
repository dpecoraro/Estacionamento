using System;

namespace GreenVille.Domain.DTO
{
    public class RptEntradaSaidaVeiculoDTO
    {

        public DateTime Momento { get; set; }

        public bool Entrada { get; set; }

        public float ValorPago { get; set; }

        public bool Mensalista { get; set; }

        public string VeiculoPlaca { get; set; }

        public string VagaNome { get; set; }

        public string EstacionamentoUnidade { get; set; }

    }
}
