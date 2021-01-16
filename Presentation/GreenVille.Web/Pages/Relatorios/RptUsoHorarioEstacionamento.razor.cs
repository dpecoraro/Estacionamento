
using GreenVille.Domain.DTO;
using GreenVille.Domain.Interfaces.IServices;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Relatorios
{
    public partial class RptUsoHorarioEstacionamento
    {

        private const string ERR_MSG_RETRIEVING_DATA = "Ocorreu um erro ao requisitar os dados no servidor. Messagem: {0}";

        EstacionamentoDTO _estacionamentoForm;
        DateTime _dataInicio;
        DateTime _dataFim;

        DateTime _extractDataInic;
        DateTime _extractDataFim;

        List<RptUsoHoraEstacionamento> UsoEstacionamentoList { get; set; }

        string _errorMessage;


        [Inject]
        IEstacionamentoApiClient EstacService { get; set; }

        [Inject]
        IRelatoriosApiClient RelatoriosService { get; set; }


        public RptUsoHorarioEstacionamento()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _errorMessage = string.Empty;
                _estacionamentoForm = new EstacionamentoDTO();
                UsoEstacionamentoList = null;

                _dataInicio = DateTime.Today;
                _dataFim = DateTime.Today.AddDays(1);

                await Task.Delay(1);
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
                var formatedDataInic = new DateTime(_dataInicio.Year, _dataInicio.Month, _dataInicio.Day, 0, 0, 0);
                var formatedDataFim = new DateTime(_dataFim.Year, _dataFim.Month, _dataFim.Day, 0, 0, 0).AddDays(1);

                if (formatedDataInic >= formatedDataFim)
                {
                    _errorMessage = "A Data Final do período precisa ser maior que a inicial!";
                }
                else if (formatedDataInic >= DateTime.Now)
                {
                    _errorMessage = "A Data Inicial do período não pode ser maior que a data atual!";
                }
                else
                {
                    _errorMessage = string.Empty;
                    _extractDataInic = _dataInicio;
                    _extractDataFim = _dataFim;


                    UsoEstacionamentoList = await RelatoriosService.GetUsoPorHorario(formatedDataInic, formatedDataFim);
                }
            }
            catch (Exception err)
            {
                _errorMessage = string.Format(ERR_MSG_RETRIEVING_DATA, err.Message);
            }
        }

    }
}
