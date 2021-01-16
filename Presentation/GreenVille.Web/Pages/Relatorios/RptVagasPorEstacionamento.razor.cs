
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Relatorios
{
    public partial class RptVagasPorEstacionamento
    {

        private const string ERR_MSG_RETRIEVING_DATA = "Ocorreu um erro ao requisitar os dados no servidor. Messagem: {0}";


        List<EstacionamentoDTO> ListagemEstacionamentos;
        int _estacionamentoId;
        string _estacionamentoNome;

        List<VagaDTO> VagasList { get; set; }

        string _errorMessage;


        [Inject]
        IVagaApiClient VagaService { get; set; }

        [Inject]
        IEstacionamentoApiClient EstacService { get; set; }


        public RptVagasPorEstacionamento()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _estacionamentoId = 0;
                VagasList = null;
                ListagemEstacionamentos = null;
                _errorMessage = string.Empty;
                _estacionamentoNome = string.Empty;

                ListagemEstacionamentos = await EstacService.GetAllAsync();
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
                if (_estacionamentoId <= 0)
                {
                    _errorMessage = "É necessário selecionar-se um Estacionamento!";
                }
                else
                {
                    _errorMessage = string.Empty;
                    _estacionamentoNome = ListagemEstacionamentos.FirstOrDefault(x => x.Id == _estacionamentoId).NomeUnidade;

                    VagasList = await VagaService.GetVagasByEstacionamento(_estacionamentoId);
                }
            }
            catch (Exception err)
            {
                _errorMessage = string.Format(ERR_MSG_RETRIEVING_DATA, err.Message);
            }
        }

    }
}
