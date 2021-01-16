using Blazored.LocalStorage;
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services
{
    public class AlocacaoApiClient : BaseApiClient<AlocacaoDTO>, IAlocacaoApiClient
    {

        private const string DOMAIN_URL = "alocacoes";
        private const string GET_BY_ESTACIONAMENTO_URL = "ListarPorEstacionamento";
        private const string GET_BY_PLATENUMBER_URL = "ListarPorPlaca";

        public AlocacaoApiClient(HttpClient httpClient, AppConfiguration appConfig, ILocalStorageService localStorage)
            : base(httpClient, appConfig, localStorage, DOMAIN_URL)
        {
        }


        public async Task<List<VeiculoAlocadoDTO>> GetAlocacoesPorEstacionamento(int idEstacionamento)
        {
            try
            {
                var alocacaoPorEstacionamentoUrl = string.Format("{0}/{1}/{2}/{3}",
                                                        _appConfig.GetApiURL(),
                                                        DOMAIN_URL,
                                                        GET_BY_ESTACIONAMENTO_URL,
                                                        idEstacionamento.ToString());
                var veiculosList = await _httpClient.GetFromJsonAsync<List<VeiculoAlocadoDTO>>(alocacaoPorEstacionamentoUrl);

                return veiculosList;
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        public async Task<AlocacaoDTO> GetAlocacaoPorPlaca(string plateNumber)
        {
            try
            {
                var alocacaoPorPlacaUrl = string.Format("{0}/{1}/{2}?plateNumber={3}",
                                                        _appConfig.GetApiURL(),
                                                        DOMAIN_URL,
                                                        GET_BY_PLATENUMBER_URL,
                                                        plateNumber);
                var alocacaoAtual = await _httpClient.GetFromJsonAsync<AlocacaoDTO>(alocacaoPorPlacaUrl);

                return alocacaoAtual;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

    }
}
