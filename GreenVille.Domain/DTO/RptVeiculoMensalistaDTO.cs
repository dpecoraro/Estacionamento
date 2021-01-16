using System.Collections.Generic;

namespace GreenVille.Domain.DTO
{
    public class RptVeiculoMensalistaDTO
    {

        public int Id { get; set; }

        public string Placa { get; set; }

        public string Modelo { get; set; }

        public string Cor { get; set; }

        public int Ano { get; set; }

        public bool Mensalista { get; set; }

        public ICollection<ClienteDTO> Proprietarios { get; set; }

    }
}
