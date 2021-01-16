using Blazored.LocalStorage;
using GreenVille.Domain.DTO;
using GreenVille.Portal.Security;
using GreenVille.Portal.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services
{
    public class AuthApiClient : BaseApiClient<AuthUserDTO>, IAuthApiClient
    {
        private const string DOMAIN_URL = "authentication/login";

        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthApiClient(HttpClient httpClient, AppConfiguration appConfig, AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage) : base(httpClient, appConfig, localStorage, DOMAIN_URL)
        {
            _authStateProvider = authStateProvider;
        }

        public async Task<AuthResponseDTO> Login(AuthUserDTO userForAuthentication)
        {
            var loginUrl = string.Format("{0}/{1}", _appConfig.GetApiURL(), DOMAIN_URL);

            var payload = new StringContent(JsonConvert.SerializeObject(userForAuthentication), Encoding.UTF8, "application/json");
            var request = await _httpClient.PostAsync(loginUrl, payload);

            var authContent = await request.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<AuthResponseDTO>(authContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!request.IsSuccessStatusCode)
            {
                return result;
            }

            await _localStorage.SetItemAsync("authToken", result.Token);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(userForAuthentication.Email);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

            return new AuthResponseDTO { IsAuthSuccessful = true };
        }


        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();

            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

    }
}
