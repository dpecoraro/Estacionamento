
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Vagas
{
    public partial class Edit
    {

        private const string ERR_MSG_RETRIEVING_DATA = "Ocorreu um erro ao requisitar os dados no servidor. Messagem: {0}";
        private const string ERR_MSG_FAILED_TO_UPDATE = "Ocorreu um erro ao tentar atualizar a Vaga. Messagem: {0}";

        private const string MSG_UPDATE_SUCCESS = "Registro atualizado com sucesso!";


        [Parameter]
        public int vagaId { get; set; }
        string ErrorMessage { get; set; }

        VagaDTO vaga = new VagaDTO();


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IVagaApiClient VagaApiClient { get; set; }


        public Edit()
        { }

        protected async override Task OnParametersSetAsync()
        {
            ErrorMessage = string.Empty;

            try
            {
                vaga = await VagaApiClient.GetByIdAsync(vagaId);
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

        async Task EditVaga()
        {
            ErrorMessage = string.Empty;

            try
            {
                var updateResponse = await VagaApiClient.UpdateAsync(vaga.Id, vaga);

                if (updateResponse.Key)
                {
                    await JSRuntime.InvokeVoidAsync("alert", MSG_UPDATE_SUCCESS);
                    uriHelper.NavigateTo("vagas");
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
