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
    public class FuncionarioApiClient : BaseApiClient<FuncionarioDTO>, IFuncionarioApiClient
    {

        private const string DOMAIN_URL = "funcionarios";

        public FuncionarioApiClient(HttpClient httpClient, AppConfiguration appConfig, ILocalStorageService localStorage)
            : base(httpClient, appConfig, localStorage, DOMAIN_URL)
        {
        }

        public async Task<List<FuncionarioDTO>> GetFuncionariosByEstacionamento(int idEstacionamento)
        {
            try
            {
                var funcionariosPorEstacionamentoUrl = string.Format("{0}/{1}?idEstacionamento={2}",
                                                        _appConfig.GetApiURL(),
                                                        DOMAIN_URL,
                                                        idEstacionamento.ToString());
                var funcionariosList = await _httpClient.GetFromJsonAsync<List<FuncionarioDTO>>(funcionariosPorEstacionamentoUrl);

                return funcionariosList;
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        public async Task<List<CargoDTO>> GetAllCargosAsync()
        {
            try
            {
                var cargosRequestUrl = string.Format("{0}/{1}", this._appConfig.GetApiURL(), "cargos");
                var cargosList = await _httpClient.GetFromJsonAsync<List<CargoDTO>>(cargosRequestUrl);

                return cargosList;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
