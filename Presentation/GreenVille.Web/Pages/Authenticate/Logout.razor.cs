using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Authenticate
{

    public partial class Logout
    {

        [Inject]
        public IAuthApiClient AuthenticationService { get; set; }

        [Inject]
        public NavigationManager uriHelper { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await AuthenticationService.Logout();
            uriHelper.NavigateTo("/");
        }

    }

}