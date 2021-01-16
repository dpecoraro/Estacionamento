
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Veiculos
{
    public partial class Edit
    {

        private const string ERR_MSG_RETRIEVING_DATA = "Ocorreu um erro ao requisitar os dados no servidor. Messagem: {0}";
        private const string ERR_MSG_FAILED_TO_UPDATE = "Ocorreu um erro ao tentar atualizar o Veiculo. Messagem: {0}";

        private const string MSG_UPDATE_SUCCESS = "Registro atualizado com sucesso!";


        [Parameter]
        public int veiculoId { get; set; }
        string ErrorMessage { get; set; }

        int ClienteId { get; set; }

        VeiculoDTO veiculo = new VeiculoDTO();


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IVeiculoApiClient VeiculoApiClient { get; set; }


        public Edit()
        { }

        protected async override Task OnParametersSetAsync()
        {
            ErrorMessage = string.Empty;

            try
            {
                veiculo = await VeiculoApiClient.GetByIdAsync(veiculoId);

                if ((null != veiculo.VeiculosClientes) && (veiculo.VeiculosClientes.Count > 0))
                {
                    veiculo.ClienteId = veiculo.VeiculosClientes.First().ClienteId;
                }
                else
                {
                    veiculo.ClienteId = 0;
                }
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

        async Task EditVeiculo()
        {
            ErrorMessage = string.Empty;

            try
            {
                var veiculoCliente = new VeiculoClienteDTO
                {
                    ClienteId = veiculo.ClienteId,
                    VeiculoId = veiculo.Id
                };

                veiculo.VeiculosClientes = new List<VeiculoClienteDTO>() { veiculoCliente };


                var updateResponse = await VeiculoApiClient.UpdateAsync(veiculo.Id, veiculo);

                if (updateResponse.Key)
                {
                    await JSRuntime.InvokeVoidAsync("alert", MSG_UPDATE_SUCCESS);
                    uriHelper.NavigateTo("veiculos");
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
