
using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Veiculos
{
    public partial class Create
    {

        private const string ERR_MSG_FAILED_TO_CREATE = "Ocorreu um erro ao tentar criar o Veiculo. Messagem: {0}";
        private const string ERR_MSG_FAILED_TO_CREATE_PLATENUMBER_ALREADY_EXISTS = "Já existe um Veículo cadastrado com a mesma placa! Por favor, selecione outra placa.";

        private const string MSG_CREATED_SUCCESS = "Registro criado com sucesso!";


        string ErrorMessage { get; set; }

        int ClienteId { get; set; }

        VeiculoDTO veiculo = new VeiculoDTO();


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IVeiculoApiClient VeiculoApiClient { get; set; }


        public Create()
        { }


        async Task CreateVeiculo()
        {
            ErrorMessage = string.Empty;

            try
            {
                //valida se placa já não está em uso
                var veiculoExistente = await VeiculoApiClient.GetVeiculoPorPlaca(veiculo.Placa);
                if (veiculoExistente != null)
                {
                    ErrorMessage = ERR_MSG_FAILED_TO_CREATE_PLATENUMBER_ALREADY_EXISTS;
                    return;
                }

                var veiculoCliente = new VeiculoClienteDTO
                {
                    ClienteId = veiculo.ClienteId,
                    VeiculoId = veiculo.Id
                };
                veiculo.VeiculosClientes = new List<VeiculoClienteDTO>() { veiculoCliente };


                var createResponse = await VeiculoApiClient.SaveAsync(veiculo);

                if (createResponse.Key)
                {
                    await JSRuntime.InvokeVoidAsync("alert", MSG_CREATED_SUCCESS);
                    uriHelper.NavigateTo("veiculos");
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
