using GreenVille.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Domain.Interfaces.IServices
{
    public interface IClientesService
    {

        Task<IEnumerable<ClienteDTO>> GetClientes();

        Task<ClienteDTO> GetCliente(int id);

        Task<ClienteDTO> AddCliente(ClienteDTO cliente);

        Task<ClienteDTO> UpdateCliente(ClienteDTO cliente);

        Task<bool> DeleteCliente(int id);

        Task<ClienteDTO> GetClientePorCPF(string cpf);

    }
}
