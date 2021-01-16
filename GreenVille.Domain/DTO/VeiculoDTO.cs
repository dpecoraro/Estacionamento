using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenVille.Domain.DTO
{
    public class VeiculoDTO
    {

        public int Id { get; set; }


        [Required(ErrorMessage = "Placa é obrigatório")]
        [MaxLength(7, ErrorMessage = "Placa deve conter no máximo 7 caracteres (sem espaços ou traços)")]
        public string Placa { get; set; }

        public string Modelo { get; set; }

        public string Cor { get; set; }


        [Required(ErrorMessage = "Ano do veículo é obrigatório")]
        [Range(1900, int.MaxValue, ErrorMessage = "Valor inválido para Ano do veículo")]
        public int Ano { get; set; }

        public bool Mensalista { get; set; }


        public ICollection<VeiculoClienteDTO> VeiculosClientes { get; set; }


        [Required(ErrorMessage = "É obrigatório selecionar-se um Proprietário")]
        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar-se um Proprietário")]
        public int ClienteId { get; set; }
    }
}
