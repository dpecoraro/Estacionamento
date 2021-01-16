
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Relatorios
{
    public partial class RptMovimentacaoVeiculoPeriodo
    {

        private const string ERR_MSG_RETRIEVING_DATA = "Ocorreu um erro ao requisitar os dados no servidor. Messagem: {0}";

        VeiculoDTO _veiculoForm;
        List<VeiculoDTO> ListagemVeiculos;
        int _veiculoId;
        string _veiculoPlaca;

        DateTime _dataInicio;
        DateTime _dataFim;

        List<RptEntradaSaidaVeiculoDTO> MovimentacaoList { get; set; }

        string _errorMessage;


        [Inject]
        IVeiculoApiClient VeicService { get; set; }

        [Inject]
        IRelatoriosApiClient RelatoriosService { get; set; }


        public RptMovimentacaoVeiculoPeriodo()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _veiculoForm = new VeiculoDTO();
                ListagemVeiculos = null;
                MovimentacaoList = null;
                _veiculoId = 0;
                _veiculoPlaca = string.Empty;
                _errorMessage = string.Empty;

                _dataInicio = DateTime.Today;
                _dataFim = DateTime.Today.AddDays(1);


                ListagemVeiculos = await VeicService.GetAllAsync();
            }
            catch (Exception err)
            {
                _errorMessage = string.Format(ERR_MSG_RETRIEVING_DATA, err.Message);
            }
        }

        async Task GetReport()
        {
            try
            {
                if (_veiculoId <= 0)
                {
                    _errorMessage = "É necessário selecionar-se um Veículo!";
                }
                else if (_dataInicio >= _dataFim)
                {
                    _errorMessage = "A Data Final do período precisa ser maior que a inicial!";
                }
                else if (_dataInicio >= DateTime.Now)
                {
                    _errorMessage = "A Data Inicial do período não pode ser maior que a data atual!";
                }
                else
                {
                    _errorMessage = string.Empty;
                    _veiculoPlaca = ListagemVeiculos.FirstOrDefault(x => x.Id == _veiculoId).Placa;

                    MovimentacaoList = await RelatoriosService.GetAtividadeVeiculosPorPeriodo(_veiculoId, _dataInicio, _dataFim);
                }
            }
            catch (Exception err)
            {
                _errorMessage = string.Format(ERR_MSG_RETRIEVING_DATA, err.Message);
            }
        }

        private async Task VeiculoList_Change(int value)
        {
            _veiculoId = value;
            await Task.Delay(1);
        }

    }
}
