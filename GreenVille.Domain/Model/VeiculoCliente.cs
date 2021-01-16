namespace GreenVille.Domain.Model
{
    public class VeiculoCliente
    {
        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }


        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
