using Blazored.LocalStorage;
using GreenVille.Portal.Security;
using GreenVille.Portal.Services;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace GreenVille.Portal
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //Add configuration file in wwwroot
            builder.Services.AddSingleton(config => ReadAppConfig());


            //Configure HttpClients and APIClients
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IFuncionarioApiClient, FuncionarioApiClient>();
            builder.Services.AddScoped<IEstacionamentoApiClient, EstacionamentoApiClient>();
            builder.Services.AddScoped<IUsuarioApiClient, UsuarioApiClient>();
            builder.Services.AddScoped<IClienteApiClient, ClienteApiClient>();
            builder.Services.AddScoped<IVeiculoApiClient, VeiculoApiClient>();
            builder.Services.AddScoped<IVagaApiClient, VagaApiClient>();
            builder.Services.AddScoped<IAuthApiClient, AuthApiClient>();
            builder.Services.AddScoped<IAlocacaoApiClient, AlocacaoApiClient>();
            builder.Services.AddScoped<IRelatoriosApiClient, RelatoriosApiClient>();


            //Enable Authentication
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();


            await builder.Build().RunAsync();
        }

        private static AppConfiguration ReadAppConfig()
        {
            string fileName = "GreenVille.Portal.appsettings.json";
            var stream = Assembly.GetExecutingAssembly()
                                 .GetManifestResourceStream(fileName);

            var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build()
                    .GetSection("AppConfiguration")
                    .Get<AppConfiguration>();

            return config;
        }
    }
}
