namespace GreenVille.Domain.Model
{
    public class Funcionario
    {

        public int Id { get; set; }

        public string CPF { get; set; }

        public string Nome { get; set; }


        public int CargoId { get; set; }
        public Cargo Cargo { get; set; }


        public int EstacionamentoId { get; set; }
        public Estacionamento Estacionamento { get; set; }

    }
}
