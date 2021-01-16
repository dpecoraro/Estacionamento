using GreenVille.Domain.DTO;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Authenticate
{
    public partial class Login
    {
        
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IAuthApiClient AuthApiClient { get; set; }

        private AuthUserDTO _usuarioAuthentication = new AuthUserDTO();


        public string ErrorMessage { get; set; }

        public bool ShowAuthError { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }


        public Login() { }

        public async Task ExecuteLogin()
        {
            try
            {
                ShowAuthError = false;
                var result = await AuthApiClient.Login(_usuarioAuthentication);

                if (!result.IsAuthSuccessful)
                {
                    ErrorMessage = result.ErrorMessage;
                    ShowAuthError = true;
                }
                else
                {
                    uriHelper.NavigateTo("/");
                }

            }
            catch (Exception err)
            {
                ErrorMessage = err.Message;
                ShowAuthError = true;
            }
        }

    }
}