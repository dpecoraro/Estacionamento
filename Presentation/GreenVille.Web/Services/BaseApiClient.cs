using Blazored.LocalStorage;
using GreenVille.Portal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services
{
    public abstract class BaseApiClient<T> : IBaseApiClient<T> where T : class
    {
        internal string _fullRequestUrl = string.Empty;

        internal HttpClient _httpClient { get; }

        internal AppConfiguration _appConfig { get; }

        internal ILocalStorageService _localStorage;

        internal string _domainUrl { get; private set; }

        internal string requestToken;


        internal BaseApiClient(HttpClient httpClient, AppConfiguration appConfig, ILocalStorageService localStorage, string domainUrl)
        {
            _appConfig = appConfig;
            _domainUrl = domainUrl;
            _localStorage = localStorage;


            _fullRequestUrl = string.Format("{0}/{1}", this._appConfig.GetApiURL(), _domainUrl);

            httpClient.BaseAddress = new Uri(_fullRequestUrl);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "GreenVille.Portal");
            _httpClient = httpClient;
        }



        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                Console.WriteLine("Teste 2");
                CheckForToken();

                Console.WriteLine("Teste 3: "+ _fullRequestUrl);
                var retrievedList = await _httpClient.GetFromJsonAsync<List<T>>(_fullRequestUrl);

                return retrievedList;
            }
            catch (Exception err)
            {
                Console.WriteLine("Erro: " + err.Message);
                if (err.InnerException != null)
                {
                    Console.WriteLine("Inner Erro: " + err.InnerException.Message);
                }

                throw err;
            }
        }

        public async Task<T> GetByIdAsync(object id)
        {
            try
            {
                CheckForToken();

                var getByIdUrl = string.Format("{0}/{1}", _fullRequestUrl, id.ToString());
                var retrievedRegister = await _httpClient.GetFromJsonAsync<T>(getByIdUrl);

                return retrievedRegister;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public async Task<KeyValuePair<bool, string>> SaveAsync(T obj)
        {
            try
            {
                CheckForToken();

                var createResponse = await _httpClient.PostAsJsonAsync(_fullRequestUrl, obj);

                if (createResponse.IsSuccessStatusCode)
                {
                    return new KeyValuePair<bool, string>(true, await createResponse.Content.ReadAsStringAsync());
                }
                else
                {
                    return new KeyValuePair<bool, string>(false, createResponse.ReasonPhrase);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public async Task<KeyValuePair<bool, string>> UpdateAsync(object id, T obj)
        {
            try
            {
                CheckForToken();

                var putByIdUrl = string.Format("{0}/{1}", _fullRequestUrl, id.ToString());
                var updateResponse = await _httpClient.PutAsJsonAsync(putByIdUrl, obj);

                if (updateResponse.IsSuccessStatusCode)
                {
                    return new KeyValuePair<bool, string>(true, await updateResponse.Content.ReadAsStringAsync());
                }
                else
                {
                    return new KeyValuePair<bool, string>(false, updateResponse.ReasonPhrase);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public async Task<KeyValuePair<bool, string>> DeleteAsync(object id)
        {
            try
            {
                CheckForToken();

                var deleteByUrl = string.Format("{0}/{1}", _fullRequestUrl, id.ToString());
                var deleteResponse = await _httpClient.DeleteAsync(deleteByUrl);

                if (deleteResponse.IsSuccessStatusCode)
                {
                    return new KeyValuePair<bool, string>(true, await deleteResponse.Content.ReadAsStringAsync());
                }
                else
                {
                    return new KeyValuePair<bool, string>(false, deleteResponse.ReasonPhrase);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        private async void CheckForToken()
        {
            if (string.IsNullOrEmpty(requestToken))
            {
                requestToken = await _localStorage.GetItemAsync<string>("authToken");
            }
            if (!string.IsNullOrEmpty(requestToken))
            {
                Console.WriteLine("RequestToken: " + requestToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", requestToken);
            }
        }
    }
}
