using GreenVille.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Domain.Interfaces.IServices
{
    public interface IVeiculosService
    {

        Task<IEnumerable<VeiculoDTO>> GetVeiculos();

        Task<VeiculoDTO> GetVeiculo(int id);

        Task<VeiculoDTO> GetVeiculoByPlateNumber(string placa);

        Task<VeiculoDTO> AddVeiculo(VeiculoDTO veiculo);

        Task<VeiculoDTO> UpdateVeiculo(VeiculoDTO veiculo);

        Task<bool> DeleteVeiculo(int id);

        Task<IEnumerable<RptVeiculoMensalistaDTO>> GetVeiculosMensalistas();

        Task<IEnumerable<VeiculoDTO>> GetVeiculosNotParked();

    }
}
