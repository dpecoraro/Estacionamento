using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Usuarios
{
    public partial class FetchData
    {

        private const string ERR_MSG_RETRIEVING_DATA = "Ocorreu um erro ao requisitar os dados no servidor. Messagem: {0}";
        private const string ERR_MSG_DELETING_RECORD = "Ocorreu um erro tentar-se deletar o registro. Messagem: {0}";

        private const string MSG_CONFIRM_DELETE = "Deseja realmente excluir o Usuário '{0}'?";
        private const string MSG_DELETE_SUCCESS = "Usuário excluído com sucesso!";


        List<UsuarioDTO> Usuarios { get; set; }

        string ErrorMessage { get; set; }


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IUsuarioApiClient UsuarioApiClient { get; set; }


        public FetchData()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Usuarios = null;
                ErrorMessage = string.Empty;

                Usuarios = await UsuarioApiClient.GetAllAsync();
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

        async Task Delete(int usuarioId)
        {
            try
            {
                var usuario = Usuarios.First(x => x.Id == usuarioId);
                if (await JSRuntime.InvokeAsync<bool>("confirm", string.Format(MSG_CONFIRM_DELETE, usuario.Nome)))
                {
                    var deleteResponse = await UsuarioApiClient.DeleteAsync(usuarioId);

                    if (deleteResponse.Key)
                    {
                        await JSRuntime.InvokeVoidAsync("alert", MSG_DELETE_SUCCESS);
                        await OnInitializedAsync();
                    }
                    else
                    {
                        ErrorMessage = string.Format(ERR_MSG_DELETING_RECORD, deleteResponse.Value);
                    }
                }
            }
            catch (Exception err)
            {
                var errorText = new StringBuilder();
                errorText.AppendFormat(ERR_MSG_DELETING_RECORD, err.Message);

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
