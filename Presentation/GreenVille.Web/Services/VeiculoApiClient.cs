using Blazored.LocalStorage;
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services
{
    public class VeiculoApiClient : BaseApiClient<VeiculoDTO>, IVeiculoApiClient
    {

        private const string DOMAIN_URL = "veiculos";
        private const string GET_BY_PLATE_NUMBER_URL = "ObterPorPlaca";
        private const string GET_VEHICLES_NOT_PARKED = "ListarVeiculosNaoEstacionados";

        public VeiculoApiClient(HttpClient httpClient, AppConfiguration appConfig, ILocalStorageService localStorage)
            : base(httpClient, appConfig, localStorage, DOMAIN_URL)
        {
        }

        public async Task<VeiculoDTO> GetVeiculoPorPlaca(string placa)
        {
            try
            {
                var veiculosPorPlacaUrl = string.Format("{0}/{1}/{2}?placa={3}",
                                                        _appConfig.GetApiURL(),
                                                        DOMAIN_URL,
                                                        GET_BY_PLATE_NUMBER_URL,
                                                        placa);
                var veiculo = await _httpClient.GetFromJsonAsync<VeiculoDTO>(veiculosPorPlacaUrl);

                return veiculo;
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

        public async Task<List<VeiculoDTO>> GetVeiculosNaoEstacionados()
        {
            try
            {
                var veiculosNaoEstacionadosUrl = string.Format("{0}/{1}/{2}",
                                                        _appConfig.GetApiURL(),
                                                        DOMAIN_URL,
                                                        GET_VEHICLES_NOT_PARKED);
                var veiculos = await _httpClient.GetFromJsonAsync<List<VeiculoDTO>>(veiculosNaoEstacionadosUrl);

                return veiculos;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
