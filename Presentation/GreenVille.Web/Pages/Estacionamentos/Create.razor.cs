
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Estacionamentos
{
    public partial class Create
    {

        private const string ERR_MSG_FAILED_TO_CREATE = "Ocorreu um erro ao tentar criar o Funcionário. Messagem: {0}";

        private const string MSG_CREATED_SUCCESS = "Registro criado com sucesso!";


        string ErrorMessage { get; set; }

        EstacionamentoDTO estac = new EstacionamentoDTO();


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IEstacionamentoApiClient EstacionamentoApiClient { get; set; }


        public Create()
        { }


        async Task CreateEstacionamento()
        {
            ErrorMessage = string.Empty;

            try
            {
                var createResponse = await EstacionamentoApiClient.SaveAsync(estac);

                if (createResponse.Key)
                {
                    await JSRuntime.InvokeVoidAsync("alert", MSG_CREATED_SUCCESS);
                    uriHelper.NavigateTo("estacionamentos");
                }
                else
                {
                    ErrorMessage = string.Format(ERR_MSG_FAILED_TO_CREATE, createResponse.Value);
                }
            }
            catch (Exception err)
            {
                var errorText = new StringBuilder();
                errorText.AppendFormat(ERR_MSG_FAILED_TO_CREATE, err.Message);

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
