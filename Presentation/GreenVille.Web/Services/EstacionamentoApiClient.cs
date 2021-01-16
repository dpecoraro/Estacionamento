using Blazored.LocalStorage;
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using System.Net.Http;

namespace GreenVille.Portal.Services
{
    public class EstacionamentoApiClient : BaseApiClient<EstacionamentoDTO>, IEstacionamentoApiClient
    {

        private const string DOMAIN_URL = "estacionamentos";

        public EstacionamentoApiClient(HttpClient httpClient, AppConfiguration appConfig, ILocalStorageService localStorage)
            : base(httpClient, appConfig, localStorage, DOMAIN_URL)
        {
        }

    }
}
