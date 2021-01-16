using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Vagas
{
    public partial class FormVaga
    {

        private const string ERR_MSG_RETRIEVING_ESTACIONAMENTOS_DATA = "Ocorreu um erro ao requisitar os dados de Estacionamentos no servidor. Messagem: {0}";


        private List<EstacionamentoDTO> Estacionamentos { get; set; }



        [Parameter]
        public VagaDTO vaga { get; set; }


        [Parameter]
        public EventCallback OnValidSubmit { get; set; }


        [Parameter]
        public string ErrorMessage { get; set; }


        [Inject]
        IEstacionamentoApiClient EstacService { get; set; }


        public FormVaga()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ErrorMessage = string.Empty;

                Estacionamentos = await EstacService.GetAllAsync();
            }
            catch (Exception err)
            {
                ErrorMessage = string.Format(ERR_MSG_RETRIEVING_ESTACIONAMENTOS_DATA, err.Message);
            }
        }
    }
}
