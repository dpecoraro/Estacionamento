using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Realocacao
{
    public partial class RealocarVeiculo
    {

        private const string ERR_MSG_FAILED_TO_CREATE = "Ocorreu um erro ao tentar realocar-se o veículo. Messagem: {0}";
        private const string ERR_MSG_FAILED_TO_FIND_ALLOCATION = "Ocorreu um erro ao tentar-se encontrar a Alocação para o Veículo selecionado. Messagem: {0}";
        private const string ERR_MSG_FAILED_TO_RETRIEVE_DATA = "Ocorreu um erro ao tentar obter dados para as listagens. Messagem: {0}";
        private const string ERR_MSG_FAILED_TO_RETRIEVE_ALLOCATION = "Não foram encontradas alocações para a placa digitada.";
        private const string ERR_MSG_VEHICLE_PLATE_MUST_BE_PROVIDED = "A placa do veículo deve ser preenchida para buscar-se a alocação atual.";
        private const string ERR_MSG_VEHICLE_PLATE_MUST_BE_7_CHARS_OR_LESS = "Placa deve conter no máximo 7 caracteres (sem espaços ou traços)";
        private const string ERR_MSG_SELECT_A_PROPER_VAGA = "É necessário selecionar-se uma nova vaga corretamente.";

        private const string MSG_CREATED_SUCCESS = "Veículo Realocado com Sucesso.";
        string _errorMessage;
        bool _alocacaoEncontrada;
        string _veiculoPlaca;

        AlocacaoDTO _alocacao = new AlocacaoDTO();

        List<VagaDTO> ListagemVagas;


        [Inject]
        IJSRuntime JSRuntime { get; set; }


        [Inject]
        IVagaApiClient VagaService { get; set; }

        [Inject]
        IAlocacaoApiClient AlocService { get; set; }


        public RealocarVeiculo()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _errorMessage = string.Empty;
                _veiculoPlaca = string.Empty;
                _alocacaoEncontrada = false;
                ListagemVagas = null;

                await Task.Delay(1);
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
        async Task BuscarAlocacao()
        {
            _errorMessage = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(_veiculoPlaca))
                {
                    throw new Exception(ERR_MSG_VEHICLE_PLATE_MUST_BE_PROVIDED);
                }
                if (_veiculoPlaca.Length > 7)
                {
                    throw new Exception(ERR_MSG_VEHICLE_PLATE_MUST_BE_7_CHARS_OR_LESS);
                }

                _alocacao = await AlocService.GetAlocacaoPorPlaca(_veiculoPlaca);
                if (null != _alocacao)
                {
                    _alocacaoEncontrada = true;

                    var vagas = await VagaService.GetAllAsync();
                    ListagemVagas = vagas.Where(x => x.EstacionamentoId == _alocacao.EstacionamentoId && !x.Ocupada && !x.Interditada).ToList();
                    _alocacao.VagaId = 0;
                }
                else
                {
                    _alocacaoEncontrada = false;
                    ListagemVagas = null;
                    _errorMessage = ERR_MSG_FAILED_TO_RETRIEVE_ALLOCATION;
                }
            }
            catch (Exception err)
            {
                var errorText = new StringBuilder();
                errorText.AppendFormat(ERR_MSG_FAILED_TO_FIND_ALLOCATION, err.Message);

                if (null != err.InnerException)
                {
                    errorText.AppendLine("-");
                    errorText.AppendFormat("Exceção Detalhes: {0}", err.InnerException.Message);
                }

                _errorMessage = errorText.ToString();
            }
        }


        async Task TrocarVaga()
        {
            _errorMessage = string.Empty;

            try
            {
                if (_alocacao.VagaId <= 0)
                {
                    _errorMessage = string.Format(ERR_MSG_SELECT_A_PROPER_VAGA);
                }
                else
                {
                    var updateResponse = await AlocService.UpdateAsync(_alocacao.Id, _alocacao);

                    if (updateResponse.Key)
                    {
                        await JSRuntime.InvokeVoidAsync("alert", MSG_CREATED_SUCCESS);
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
    }
}
