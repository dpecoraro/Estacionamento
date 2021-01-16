using GreenVille.Domain.DTO;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services.Interfaces
{
    public interface IClienteApiClient: IBaseApiClient<ClienteDTO>
    {
        Task<ClienteDTO> GetClientePorCPF(string cpf);
    }
}