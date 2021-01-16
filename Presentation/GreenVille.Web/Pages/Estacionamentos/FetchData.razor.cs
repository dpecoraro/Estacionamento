
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Estacionamentos
{
    public partial class FetchData
    {

        private const string ERR_MSG_RETRIEVING_DATA = "Ocorreu um erro ao requisitar os dados no servidor. Messagem: {0}";
        private const string ERR_MSG_DELETING_RECORD = "Ocorreu um erro tentar-se deletar o registro. Messagem: {0}";

        private const string MSG_CONFIRM_DELETE = "Deseja realmente excluir o Funcionário '{0}'?";
        private const string MSG_DELETE_SUCCESS = "Funcionário excluído com sucesso!";


        List<EstacionamentoDTO> Estacionamentos { get; set; }

        string ErrorMessage { get; set; }


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IEstacionamentoApiClient EstacionamentoApiClient { get; set; }


        public FetchData()
        { }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Estacionamentos = null;
                ErrorMessage = string.Empty;

                Estacionamentos = await EstacionamentoApiClient.GetAllAsync();
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

        async Task Delete(int estaconarioId)
        {
            try
            {
                var estac = Estacionamentos.First(x => x.Id == estaconarioId);
                if (await JSRuntime.InvokeAsync<bool>("confirm", string.Format(MSG_CONFIRM_DELETE, estac.NomeUnidade)))
                {
                    var deleteResponse = await EstacionamentoApiClient.DeleteAsync(estaconarioId);

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
