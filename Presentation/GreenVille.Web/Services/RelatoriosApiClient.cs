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
    public class RelatoriosApiClient : BaseApiClient<object>, IRelatoriosApiClient
    {

        private const string DOMAIN_URL = "relatorios";
        private const string RPT_VEICULOS_MENSALISTAS = "VeiculosMensalistas";
        private const string RPT_MOVIMENTACAO_VEICULO = "MovimentacaoPorVeiculo";
        private const string RPT_USO_POR_HORARIO = "UsoPorHorario";


        public RelatoriosApiClient(HttpClient httpClient, AppConfiguration appConfig, ILocalStorageService localStorage)
            : base(httpClient, appConfig, localStorage, DOMAIN_URL)
        {
        }


        public async Task<List<RptVeiculoMensalistaDTO>> GetAllVeiculosMensalistas()
        {
            try
            {
                var veiculosMensalistasUrl = string.Format("{0}/{1}/{2}",
                                                        _appConfig.GetApiURL(),
                                                        DOMAIN_URL,
                                                        RPT_VEICULOS_MENSALISTAS);
                var veiculosList = await _httpClient.GetFromJsonAsync<List<RptVeiculoMensalistaDTO>>(veiculosMensalistasUrl);

                return veiculosList;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public async Task<List<RptEntradaSaidaVeiculoDTO>> GetAtividadeVeiculosPorPeriodo(int idVeiculo, DateTime periodoInicio, DateTime periodoFim)
        {
            try
            {
                var movimentacaoVeiculoUrl = string.Format("{0}/{1}/{2}/{3}?periodoInicio={4}&periodoFim={5}",
                                                        _appConfig.GetApiURL(),
                                                        DOMAIN_URL,
                                                        RPT_MOVIMENTACAO_VEICULO,
                                                        idVeiculo.ToString(),
                                                        periodoInicio.ToString("yyyy-MM-dd HH:mm:ss"),
                                                        periodoFim.ToString("yyyy-MM-dd HH:mm:ss"));
                var veiculosList = await _httpClient.GetFromJsonAsync<List<RptEntradaSaidaVeiculoDTO>>(movimentacaoVeiculoUrl);

                return veiculosList;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public async Task<List<RptUsoHoraEstacionamento>> GetUsoPorHorario(DateTime periodoInicio, DateTime periodoFim)
        {
            try
            {
                var usoPorHorarioUrl = string.Format("{0}/{1}/{2}?periodoInicio={3}&periodoFim={4}",
                                                        _appConfig.GetApiURL(),
                                                        DOMAIN_URL,
                                                        RPT_USO_POR_HORARIO,
                                                        periodoInicio.ToString("yyyy-MM-dd HH:mm:ss"),
                                                        periodoFim.ToString("yyyy-MM-dd HH:mm:ss"));
                var usoPorHorarioList = await _httpClient.GetFromJsonAsync<List<RptUsoHoraEstacionamento>>(usoPorHorarioUrl);

                return usoPorHorarioList;
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        public new Task<List<object>> GetAllAsync() { return null; }

        public new Task<object> GetByIdAsync(object id) { return null; }


        public new Task<KeyValuePair<bool, string>> SaveAsync(object obj) { return null; }

        public new Task<KeyValuePair<bool, string>> UpdateAsync(object id, object obj) { return null; }

        public new Task<KeyValuePair<bool, string>> DeleteAsync(object id) { return null; }

    }
}
