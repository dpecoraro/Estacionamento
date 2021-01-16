using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Usuarios
{
    public partial class FormUsuarios
    {

        private const string ERR_MSG_RETRIEVING_FUNCIONARIOS_DATA = "Ocorreu um erro ao requisitar os dados de Funcionários no servidor. Messagem: {0}";


        private List<FuncionarioDTO> Funcionarios { get; set; }



        [Parameter]
        public UsuarioDTO usuario { get; set; }


        [Parameter]
        public EventCallback OnValidSubmit { get; set; }


        [Parameter]
        public string ErrorMessage { get; set; }


        [Inject]
        IFuncionarioApiClient FuncService { get; set; }


        public FormUsuarios()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ErrorMessage = string.Empty;

                Funcionarios = await FuncService.GetAllAsync();
            }
            catch (Exception err)
            {
                ErrorMessage = string.Format(ERR_MSG_RETRIEVING_FUNCIONARIOS_DATA, err.Message);
            }
        }
    }
}
