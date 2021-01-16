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
    public class VagaApiClient : BaseApiClient<VagaDTO>, IVagaApiClient
    {

        private const string DOMAIN_URL = "vagas";
        private const string RPT_VAGAS_BY_ESTACIONAMENTO = "GetVagasByEstacionamento";

        public VagaApiClient(HttpClient httpClient, AppConfiguration appConfig, ILocalStorageService localStorage)
            : base(httpClient, appConfig, localStorage, DOMAIN_URL)
        {
        }


        public async Task<List<VagaDTO>> GetVagasByEstacionamento(int idEstacionamento)
        {
            try
            {
                var vagaByEstacionamentoUrl = string.Format("{0}/{1}/{2}/{3}",
                                                        _appConfig.GetApiURL(),
                                                        DOMAIN_URL,
                                                        RPT_VAGAS_BY_ESTACIONAMENTO,
                                                        idEstacionamento.ToString());
                var vagasList = await _httpClient.GetFromJsonAsync<List<VagaDTO>>(vagaByEstacionamentoUrl);

                return vagasList;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

    }
}
