using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Retirada
{
    public partial class RetirarVeiculo
    {

        private const string ERR_MSG_FAILED_TO_CREATE = "Ocorreu um erro ao tentar realizar-se a Retirada do Veículo. Messagem: {0}";
        private const string ERR_MSG_FAILED_TO_RETRIEVE_DATA = "Ocorreu um erro ao tentar obter dados para as listagens. Messagem: {0}";

        private const string MSG_CREATED_SUCCESS = "Veículo Retirado com Sucesso. Recibo: '{0}'";

        private const int ID_CARGO_ATENDENTE = 2;
        private const int ID_CARGO_MANOBRISTA = 3;

        string _errorMessage;
        bool _confirmacao;
        string _horasCobradas;

        AlocacaoDTO _alocacao = new AlocacaoDTO();

        List<FuncionarioDTO> ListagemManobristas;
        List<FuncionarioDTO> ListagemAtendentes;


        [Parameter]
        public int alocacaoId { get; set; }


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IFuncionarioApiClient FuncService { get; set; }

        [Inject]
        IAlocacaoApiClient AlocService { get; set; }

        [Inject]
        IEstacionamentoApiClient EstacService { get; set; }


        public RetirarVeiculo()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _confirmacao = false;
                _errorMessage = string.Empty;
                ListagemAtendentes = null;
                ListagemManobristas = null;


                _alocacao = await AlocService.GetByIdAsync(alocacaoId);


                var funcionarios = await FuncService.GetFuncionariosByEstacionamento(_alocacao.EstacionamentoId);
                ListagemAtendentes = funcionarios.Where(x => x.CargoId == ID_CARGO_ATENDENTE).ToList();
                ListagemManobristas = funcionarios.Where(x => x.CargoId == ID_CARGO_MANOBRISTA).ToList();

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


        async Task CheckoutVehicle()
        {
            try
            {
                _errorMessage = string.Empty;

                //Tela tem 2 fases antes de poder salvar:
                //    1 - Confirmação dos dados e bloqueio dos campos de edição
                //    2 - Salvar os dados de retirada do veículo
                if (!_confirmacao)
                {
                    //calcula quantidade de horas estacionadas
                    _alocacao.Saida = DateTime.Now;
                    var totalHorasUso = _alocacao.Saida - _alocacao.Entrada;

                    //verifica horário negativo e descarta segundos
                    if (totalHorasUso.Value.TotalHours < 0)
                    {
                        totalHorasUso = new TimeSpan(00, 00, 00);
                    }
                    else
                    {
                        totalHorasUso = new TimeSpan(totalHorasUso.Value.Hours, totalHorasUso.Value.Minutes, 00);
                    }

                    //apresenta quantida de horas cobradas
                    _horasCobradas = string.Format("{0}:{1}", totalHorasUso.Value.Hours.ToString("00"), totalHorasUso.Value.Minutes.ToString("00"));



                    //obtêm estacionamento para pegar tarifa por hora e razão de geração de economia de carbono
                    var estacionamento = await EstacService.GetByIdAsync(_alocacao.EstacionamentoId);

                    //verifica se o veículo é Mensalista ou não, se não for, precisa calcular tarifa
                    if (!_alocacao.Mensalista)
                    {
                        var valorAPagar = totalHorasUso.Value.TotalHours * estacionamento.ValorHora;
                        if (valorAPagar < 0) { valorAPagar = 0; }
                        _alocacao.ValorPago = float.Parse(valorAPagar.ToString());
                    }
                    else
                    {
                        _alocacao.ValorPago = 0;
                    }

                    //calcula quantida de carbono economizada
                    _alocacao.EconomiaCarbono = totalHorasUso.Value.TotalHours * estacionamento.GeracaoCreditosCarbonoHora;


                    //Marca status da tela com confirmação
                    _confirmacao = true;
                }
                else
                {
                    //tenta atualizar registro da alocação com a retirada
                    var updateResponse = await AlocService.UpdateAsync(_alocacao.Id, _alocacao);

                    if (updateResponse.Key)
                    {
                        var receiptNumber = _alocacao.Id.ToString("00000000");

                        await JSRuntime.InvokeVoidAsync("alert", string.Format(MSG_CREATED_SUCCESS, receiptNumber));
                        uriHelper.NavigateTo("/");
                    }
                    else
                    {
                        _errorMessage = string.Format(ERR_MSG_FAILED_TO_CREATE, updateResponse.Value);
                    }
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

        async Task CancelarConfirmacao()
        {
            _confirmacao = false;
            await Task.Delay(1);
        }
    }
}
