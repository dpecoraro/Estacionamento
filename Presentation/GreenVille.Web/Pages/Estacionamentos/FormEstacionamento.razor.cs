using GreenVille.Domain.DTO;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace GreenVille.Portal.Pages.Estacionamentos
{
    public partial class FormEstacionamento
    {

        [Parameter]
        public EstacionamentoDTO estac { get; set; }


        [Parameter]
        public EventCallback OnValidSubmit { get; set; }


        [Parameter]
        public string ErrorMessage { get; set; }



        public FormEstacionamento()
        { }

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(1);
            ErrorMessage = string.Empty;
        }
    }
}
