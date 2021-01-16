
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Relatorios
{
    public partial class RptVeiculosMensalistas
    {

        private const string ERR_MSG_RETRIEVING_DATA = "Ocorreu um erro ao requisitar os dados no servidor. Messagem: {0}";


        List<RptVeiculoMensalistaDTO> VeiculosMensalistas { get; set; }

        string ErrorMessage { get; set; }


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IRelatoriosApiClient RelatoriosApiClient { get; set; }


        public RptVeiculosMensalistas()
        { }

        protected override void OnInitialized()
        {
            VeiculosMensalistas = null;
            ErrorMessage = string.Empty;
        }

        async Task GetReport()
        {
            try
            {
                VeiculosMensalistas = await RelatoriosApiClient.GetAllVeiculosMensalistas();
            }
            catch (Exception err)
            {
                ErrorMessage = string.Format(ERR_MSG_RETRIEVING_DATA, err.Message);
            }
        }

    }
}
