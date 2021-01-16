
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Retirada
{
    public partial class FetchData
    {

        private const string ERR_MSG_RETRIEVING_DATA = "Ocorreu um erro ao requisitar os dados no servidor. Messagem: {0}";
        private const string ERR_MSG_ESTACIONAMENTO_REQUIRED = "É necessário selecionar-se um Estacionamento!";
        VeiculoAlocadoDTO _alocacoesForm;
        List<EstacionamentoDTO> ListagemEstacionamentos;
        int _estacionamentoId;

        List<VeiculoAlocadoDTO> Alocacoes { get; set; }

        string ErrorMessage { get; set; }


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IAlocacaoApiClient AlocacaoApiClient { get; set; }

        [Inject]
        IEstacionamentoApiClient EstacService { get; set; }


        public FetchData()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _alocacoesForm = new VeiculoAlocadoDTO();

                Alocacoes = null;
                ErrorMessage = string.Empty;
                await Task.Delay(1);

                Console.WriteLine("Teste 1");

                ListagemEstacionamentos = await EstacService.GetAllAsync();

            }
            catch (Exception err)
            {
                var errorText = new StringBuilder();
                errorText.AppendFormat(ERR_MSG_RETRIEVING_DATA, err.Message);

                if (null != err.InnerException)
                {
                    errorText.AppendLine("-");
                    errorText.AppendFormat("Exceção Detalhes: {0}", err.InnerException.Message);
                }

                ErrorMessage = errorText.ToString();
            }
        }

        async Task GetAlocacoes()
        {
            try
            {
                if (_estacionamentoId <= 0)
                {
                    ErrorMessage = ERR_MSG_ESTACIONAMENTO_REQUIRED;
                    Alocacoes = null;
                }
                else
                {
                    ErrorMessage = string.Empty;
                    Alocacoes = await AlocacaoApiClient.GetAlocacoesPorEstacionamento(_estacionamentoId);
                }
            }
            catch (Exception err)
            {
                var errorText = new StringBuilder();
                errorText.AppendFormat(ERR_MSG_RETRIEVING_DATA, err.Message);

                if (null != err.InnerException)
                {
                    errorText.AppendLine("-");
                    errorText.AppendFormat("Exceção Detalhes: {0}", err.InnerException.Message);
                }

                ErrorMessage = errorText.ToString();
            }
        }

        private async Task EstacionamentoList_Change(int value)
        {
            _estacionamentoId = value;
            await Task.Delay(1);

            await GetAlocacoes();
        }

    }
}
