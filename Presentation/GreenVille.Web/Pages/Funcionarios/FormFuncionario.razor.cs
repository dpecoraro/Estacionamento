using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Funcionarios
{
    public partial class FormFuncionario
    {

        private const string ERR_MSG_RETRIEVING_CARGOS_DATA = "Ocorreu um erro ao requisitar os dados de Cargos no servidor. Messagem: {0}";


        private List<CargoDTO> Cargos { get; set; }
        private List<EstacionamentoDTO> Estacionamentos { get; set; }



        [Parameter]
        public FuncionarioDTO funci { get; set; }


        [Parameter]
        public EventCallback OnValidSubmit { get; set; }


        [Parameter]
        public string ErrorMessage { get; set; }


        [Inject]
        IFuncionarioApiClient FuncService { get; set; }

        [Inject]
        IEstacionamentoApiClient EstacService { get; set; }


        public FormFuncionario()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ErrorMessage = string.Empty;

                Cargos = await FuncService.GetAllCargosAsync();

                Estacionamentos = await EstacService.GetAllAsync();
            }
            catch (Exception err)
            {
                ErrorMessage = string.Format(ERR_MSG_RETRIEVING_CARGOS_DATA, err.Message);
            }
        }
    }
}
