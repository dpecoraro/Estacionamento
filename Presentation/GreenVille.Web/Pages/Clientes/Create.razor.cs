
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Clientes
{
    public partial class Create
    {

        private const string ERR_MSG_FAILED_TO_CREATE = "Ocorreu um erro ao tentar criar o Cliente. Messagem: {0}";
        private const string ERR_MSG_FAILED_TO_CREATE_CPF_ALREADY_EXISTS = "Já existe um Cliente cadastrado com a mesma CPF! Por favor, selecione outro CPF.";

        private const string MSG_CREATED_SUCCESS = "Registro criado com sucesso!";


        string ErrorMessage { get; set; }

        ClienteDTO cliente = new ClienteDTO();


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IClienteApiClient ClienteApiClient { get; set; }


        public Create()
        { }


        async Task CreateCliente()
        {
            ErrorMessage = string.Empty;

            try
            {
                //valida se CPF já não está em uso
                var clienteExirtente = await ClienteApiClient.GetClientePorCPF(cliente.CPF);
                if (clienteExirtente != null)
                {
                    ErrorMessage = ERR_MSG_FAILED_TO_CREATE_CPF_ALREADY_EXISTS;
                    return;
                }

                var createResponse = await ClienteApiClient.SaveAsync(cliente);

                if (createResponse.Key)
                {
                    await JSRuntime.InvokeVoidAsync("alert", MSG_CREATED_SUCCESS);
                    uriHelper.NavigateTo("clientes");
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
