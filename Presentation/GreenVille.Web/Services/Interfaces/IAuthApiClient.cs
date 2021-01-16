using GreenVille.Domain.DTO;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services.Interfaces
{
    public interface IAuthApiClient : IBaseApiClient<AuthUserDTO>
    {
        Task<AuthResponseDTO> Login(AuthUserDTO userForAuthentication);

        Task Logout();
    }
}
