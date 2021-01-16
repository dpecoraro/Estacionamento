
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Clientes
{
    public partial class FetchData
    {

        private const string ERR_MSG_RETRIEVING_DATA = "Ocorreu um erro ao requisitar os dados no servidor. Messagem: {0}";
        private const string ERR_MSG_DELETING_RECORD = "Ocorreu um erro tentar-se deletar o registro. Messagem: {0}";

        private const string MSG_CONFIRM_DELETE = "Deseja realmente excluir o Cliente '{0}'?";
        private const string MSG_DELETE_SUCCESS = "Cliente excluída com sucesso!";


        List<ClienteDTO> Clientes { get; set; }

        string ErrorMessage { get; set; }


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IClienteApiClient ClienteApiClient { get; set; }


        public FetchData()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Clientes = null;
                ErrorMessage = string.Empty;

                Clientes = await ClienteApiClient.GetAllAsync();
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

        async Task Delete(int clienteId)
        {
            try
            {
                var cliente = Clientes.First(x => x.Id == clienteId);
                if (await JSRuntime.InvokeAsync<bool>("confirm", string.Format(MSG_CONFIRM_DELETE, cliente.Nome)))
                {
                    var deleteResponse = await ClienteApiClient.DeleteAsync(clienteId);

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
