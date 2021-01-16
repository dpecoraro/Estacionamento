using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Veiculos
{
    public partial class FormVeiculo
    {

        private const string ERR_MSG_RETRIEVING_CLIENTES_DATA = "Ocorreu um erro ao requisitar os dados de Clientes no servidor. Messagem: {0}";

        private List<ClienteDTO> Clientes { get; set; }


        [Parameter]
        public VeiculoDTO veiculo { get; set; }


        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Inject]
        IClienteApiClient ClientService { get; set; }

        [Parameter]
        public string ErrorMessage { get; set; }

        [Parameter]
        public int ClienteId { get; set; }



        public FormVeiculo()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ErrorMessage = string.Empty;

                Clientes = await ClientService.GetAllAsync();
            }
            catch (Exception err)
            {
                ErrorMessage = string.Format(ERR_MSG_RETRIEVING_CLIENTES_DATA, err.Message);
            }
        }
    }
}
