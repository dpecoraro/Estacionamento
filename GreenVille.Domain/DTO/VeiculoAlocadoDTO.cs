using System;

namespace GreenVille.Domain.DTO
{
    public class VeiculoAlocadoDTO
    {
        public int Id { get; set; }

        public DateTime Entrada { get; set; }

        public string VagaNome { get; set; }

        public string VeiculoPlaca { get; set; }
    }
}
