using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Usuarios
{
    public partial class Edit
    {

        private const string ERR_MSG_RETRIEVING_DATA = "Ocorreu um erro ao requisitar os dados no servidor. Messagem: {0}";
        private const string ERR_MSG_FAILED_TO_UPDATE = "Ocorreu um erro ao tentar atualizar o Usuário. Messagem: {0}";

        private const string MSG_UPDATE_SUCCESS = "Registro atualizado com sucesso!";


        [Parameter]
        public int usuarioId { get; set; }
        string ErrorMessage { get; set; }

        UsuarioDTO usuario = new UsuarioDTO();


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IUsuarioApiClient UsuarioApiClient { get; set; }


        public Edit()
        { }


        protected async override Task OnParametersSetAsync()
        {
            ErrorMessage = string.Empty;

            try
            {
                usuario = await UsuarioApiClient.GetByIdAsync(usuarioId);
            }
            catch (Exception err)
            {
                var errorText = new StringBuilder();
                errorText.AppendFormat(ERR_MSG_RETRIEVING_DATA, err.Message);

                if (null != err.InnerException)
                {
                    errorText.AppendLine("-");
                    errorText.AppendFormat("Exceção Detalhes: {0}", err.InnerException.Message);
                }

                ErrorMessage = errorText.ToString();
            }
        }

        async Task EditUsuario()
        {
            ErrorMessage = string.Empty;

            try
            {
                usuario.FuncionarioVinculado = (usuario.FuncionarioId != 0);

                var updateResponse = await UsuarioApiClient.UpdateAsync(usuario.Id, usuario);
                if (updateResponse.Key)
                {
                    await JSRuntime.InvokeVoidAsync("alert", MSG_UPDATE_SUCCESS);
                    uriHelper.NavigateTo("usuarios");
                }
                else
                {
                    ErrorMessage = string.Format(ERR_MSG_FAILED_TO_UPDATE, updateResponse.Value);
                }
            }
            catch (Exception err)
            {
                var errorText = new StringBuilder();
                errorText.AppendFormat(ERR_MSG_FAILED_TO_UPDATE, err.Message);

                if (null != err.InnerException)
                {
                    errorText.AppendLine("-");
                    errorText.AppendFormat("Exceção Detalhes: {0}", err.InnerException.Message);
                }

                ErrorMessage = errorText.ToString();
            }
        }

    }
}
