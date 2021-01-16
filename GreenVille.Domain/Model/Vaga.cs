namespace GreenVille.Domain.Model
{
    public class Vaga
    {

        public int Id { get; set; }

        public string Nome { get; set; }

        public bool Ocupada { get; set; }

        public bool Interditada { get; set; }


        public int EstacionamentoId { get; set; }
        public Estacionamento Estacionamento { get; set; }
    }
}
