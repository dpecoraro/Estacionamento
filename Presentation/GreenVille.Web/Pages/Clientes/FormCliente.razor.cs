using GreenVille.Domain.DTO;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Clientes
{
    public partial class FormCliente
    {

        [Parameter]
        public ClienteDTO cliente { get; set; }


        [Parameter]
        public EventCallback OnValidSubmit { get; set; }


        [Parameter]
        public string ErrorMessage { get; set; }



        public FormCliente()
        { }

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(1);
            ErrorMessage = string.Empty;
        }
    }
}
