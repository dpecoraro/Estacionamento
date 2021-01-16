using Blazored.LocalStorage;
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using System.Net.Http;

namespace GreenVille.Portal.Services
{
    public class UsuarioApiClient : BaseApiClient<UsuarioDTO>, IUsuarioApiClient
    {

        private const string DOMAIN_URL = "usuarios";

        public UsuarioApiClient(HttpClient httpClient, AppConfiguration appConfig, ILocalStorageService localStorage)
            : base(httpClient, appConfig, localStorage, DOMAIN_URL)
        {
        }

    }
}
