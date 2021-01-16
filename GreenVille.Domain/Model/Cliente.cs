using System.Collections.Generic;

namespace GreenVille.Domain.Model
{
    public class Cliente
    {

        public int Id { get; set; }

        public string Nome { get; set; }

        public string Telefone { get; set; }

        public string CPF { get; set; }


        public ICollection<VeiculoCliente> VeiculosClientes { get; set; }

    }
}
