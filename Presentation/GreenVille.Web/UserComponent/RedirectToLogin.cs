using Microsoft.AspNetCore.Components;

namespace GreenVille.Portal.UserComponent
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo("/login");
        }
    }
}