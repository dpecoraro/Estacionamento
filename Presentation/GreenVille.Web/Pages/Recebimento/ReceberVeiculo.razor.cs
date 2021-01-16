using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Recebimento
{
    public partial class ReceberVeiculo
    {

        private const string ERR_MSG_FAILED_TO_CREATE = "Ocorreu um erro ao tentar Receber o Veículo. Messagem: {0}";
        private const string ERR_MSG_FAILED_TO_RETRIEVE_DATA = "Ocorreu um erro ao tentar obter dados para as listagens. Messagem: {0}";
        private const string ERR_MSG_DATETIME_ENTRANCE_BIGGER_THAN_CURRENT = "A Data/Hora de entrada não pode ser maior que a atual!";

        private const string MSG_CREATED_SUCCESS = "Veículo Recebido com Sucesso. Recibo: '{0}'";

        private const int ID_CARGO_ATENDENTE = 2;
        private const int ID_CARGO_MANOBRISTA = 3;
        string _errorMessage;

        AlocacaoDTO _alocacao = new AlocacaoDTO();

        List<EstacionamentoDTO> ListagemEstacionamentos;
        int _estacionamentoId;

        List<VagaDTO> ListagemVagas;
        List<VeiculoDTO> ListagemVeiculos;
        List<FuncionarioDTO> ListagemManobristas;
        List<FuncionarioDTO> ListagemAtendentes;



        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IFuncionarioApiClient FuncService { get; set; }

        [Inject]
        IEstacionamentoApiClient EstacService { get; set; }

        [Inject]
        IVagaApiClient VagaService { get; set; }

        [Inject]
        IVeiculoApiClient VeicService { get; set; }

        [Inject]
        IAlocacaoApiClient AlocService { get; set; }


        public ReceberVeiculo()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _errorMessage = string.Empty;
                ListagemVagas = null;
                ListagemVeiculos = null;
                ListagemEstacionamentos = null;
                ListagemAtendentes = null;
                ListagemManobristas = null;
                _alocacao.Entrada = DateTime.Now;


                ListagemEstacionamentos = await EstacService.GetAllAsync();

                ListagemVeiculos = await VeicService.GetVeiculosNaoEstacionados();
            }
            catch (Exception err)
            {
                var errorText = new StringBuilder();
                errorText.AppendFormat(ERR_MSG_FAILED_TO_RETRIEVE_DATA, err.Message);

                if (null != err.InnerException)
                {
                    errorText.AppendLine("-");
                    errorText.AppendFormat("Exceção Detalhes: {0}", err.InnerException.Message);
                }

                _errorMessage = errorText.ToString();
            }
        }


        private async Task EstacionamentoList_Change(int value)
        {
            try
            {
                _estacionamentoId = value;
                _alocacao.VagaId = 0;

                if (value == 0)
                {
                    ListagemVagas.Clear();
                }
                else
                {
                    var vagas = await VagaService.GetAllAsync();
                    ListagemVagas = vagas.Where(x => x.EstacionamentoId == _estacionamentoId && !x.Ocupada && !x.Interditada).ToList();

                    var funcionarios = await FuncService.GetFuncionariosByEstacionamento(_estacionamentoId);
                    ListagemAtendentes = funcionarios.Where(x => x.CargoId == ID_CARGO_ATENDENTE).ToList();
                    ListagemManobristas = funcionarios.Where(x => x.CargoId == ID_CARGO_MANOBRISTA).ToList();
                }
            }
            catch (Exception err)
            {
                var errorText = new StringBuilder();
                errorText.AppendFormat(ERR_MSG_FAILED_TO_RETRIEVE_DATA, err.Message);

                if (null != err.InnerException)
                {
                    errorText.AppendLine("-");
                    errorText.AppendFormat("Exceção Detalhes: {0}", err.InnerException.Message);
                }

                _errorMessage = errorText.ToString();
            }
        }


        async Task ReceiveVehicle()
        {
            _errorMessage = string.Empty;

            try
            {
                if (_alocacao.Entrada > DateTime.Now)
                {
                    _errorMessage = string.Format(ERR_MSG_DATETIME_ENTRANCE_BIGGER_THAN_CURRENT);
                    return;
                }

                var createResponse = await AlocService.SaveAsync(_alocacao);

                if (createResponse.Key)
                {
                    var alocacaoId = int.Parse(createResponse.Value);
                    var receiptNumber = alocacaoId.ToString("00000000");

                    await JSRuntime.InvokeVoidAsync("alert", string.Format(MSG_CREATED_SUCCESS, receiptNumber));
                    uriHelper.NavigateTo("/");
                }
                else
                {
                    _errorMessage = string.Format(ERR_MSG_FAILED_TO_CREATE, createResponse.Value);
                }
            }
            catch (Exception err)
            {
                var errorText = new StringBuilder();
                errorText.AppendFormat(ERR_MSG_FAILED_TO_CREATE, err.Message);

                if (null != err.InnerException)
                {
                    errorText.AppendLine("-");
                    errorText.AppendFormat("Exceção Detalhes: {0}", err.InnerException.Message);
                }

                _errorMessage = errorText.ToString();
            }
        }
    }
}
