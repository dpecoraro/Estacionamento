﻿using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Usuarios
{
    public partial class Create
    {

        private const string ERR_MSG_FAILED_TO_CREATE = "Ocorreu um erro ao tentar criar o Usuário. Messagem: {0}";

        private const string MSG_CREATED_SUCCESS = "Registro criado com sucesso!";


        string ErrorMessage { get; set; }

        UsuarioDTO usuario = new UsuarioDTO();


        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IUsuarioApiClient UsuarioApiClient { get; set; }


        public Create()
        { }


        async Task CreateUsuario()
        {
            ErrorMessage = string.Empty;

            try
            {
                usuario.FuncionarioVinculado = (usuario.FuncionarioId != 0);

                var creationResponse = await UsuarioApiClient.SaveAsync(usuario);
                if (creationResponse.Key)
                {
                    await JSRuntime.InvokeVoidAsync("alert", MSG_CREATED_SUCCESS);
                    uriHelper.NavigateTo("usuarios");
                }
                else
                {
                    ErrorMessage = string.Format(ERR_MSG_FAILED_TO_CREATE, creationResponse.Value);
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
