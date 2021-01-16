using Blazored.LocalStorage;
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services
{
    public class ClienteApiClient : BaseApiClient<ClienteDTO>, IClienteApiClient
    {

        private const string DOMAIN_URL = "clientes";
        private const string GET_BY_CPF_URL = "ObterPorCPF";

        public ClienteApiClient(HttpClient httpClient, AppConfiguration appConfig, ILocalStorageService localStorage)
            : base(httpClient, appConfig, localStorage, DOMAIN_URL)
        {
        }

        public async Task<ClienteDTO> GetClientePorCPF(string cpf)
        {
            try
            {
                var clientePorCPFUrl = string.Format("{0}/{1}/{2}?cpf={3}",
                                                        _appConfig.GetApiURL(),
                                                        DOMAIN_URL,
                                                        GET_BY_CPF_URL,
                                                        cpf);
                var cliente = await _httpClient.GetFromJsonAsync<ClienteDTO>(clientePorCPFUrl);

                return cliente;
            }
            catch (HttpRequestException httpError)
            {
                Console.WriteLine("Erro na requisição: '{0}'.", httpError.Message);
                Console.WriteLine("ErrorCode: '{0}'.", httpError.HResult);

                if (httpError.Message.Contains("404"))
                {
                    return null;
                }
                throw new Exception(httpError.Message, httpError);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

    }
}