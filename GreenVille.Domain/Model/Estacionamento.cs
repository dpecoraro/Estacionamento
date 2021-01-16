using System.Collections.Generic;

namespace GreenVille.Domain.Model
{
    public class Estacionamento
    {

        public int Id { get; set; }

        public string NomeUnidade { get; set; }

        public double ValorHora { get; set; }

        public double GeracaoCreditosCarbonoHora { get; set; }


        public ICollection<Vaga> Vagas { get; set; }

    }
}
