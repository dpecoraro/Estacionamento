using System.Collections.Generic;

namespace GreenVille.Domain.Model
{
    public class Veiculo
    {

        public int Id { get; set; }

        public string Placa { get; set; }

        public string Modelo { get; set; }

        public string Cor { get; set; }

        public int Ano { get; set; }

        public bool Mensalista { get; set; }


        public ICollection<VeiculoCliente> VeiculosClientes { get; set; }

    }
}
