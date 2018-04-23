using BuscaCep.Data.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BuscaCep.Clients
{
    class BuscaCepHttpClient
    {
        private static Lazy<BuscaCepHttpClient> _Lazy = new Lazy<BuscaCepHttpClient>(() => new BuscaCepHttpClient());
        public static BuscaCepHttpClient Current { get => _Lazy.Value; }

        private BuscaCepHttpClient()
        {
            _HttpClient = new HttpClient
            {
                BaseAddress = new Uri("http://intro-xamarin-forms.azurewebsites.net/")
            };
        }

        private readonly HttpClient _HttpClient;

        public async Task Autenticar(string usuario, string senha)
        {
            try
            {
                var param = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", usuario),
                    new KeyValuePair<string, string>("password", senha)
                };

                FormUrlEncodedContent content = new FormUrlEncodedContent(param);

                using (var response = await _HttpClient.PostAsync("token", content))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado não de deu certo ao consultar o CEP");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado não de deu certo ao consultar o CEP");

                    var token = JsonConvert.DeserializeObject<TokenResult>(result);

                    this._HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsAuthorized { get => this._HttpClient.DefaultRequestHeaders.Authorization != null; }

        public async Task<List<CepDto>> GetCeps()
        {
            try
            {
                using (var response = await _HttpClient.GetAsync("api/ceps"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                            throw new UnauthorizedAccessException("Você não está autenticado, deseja autenticar-se?");

                        throw new InvalidOperationException("Algo de errado não de deu certo ao consultar o CEP");
                    }

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado não de deu certo ao consultar o CEP");

                    return JsonConvert.DeserializeObject<List<CepDto>>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CepDto> PostCep(CepDto dto)
        {
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

                using (var response = await _HttpClient.PostAsync("api/ceps", jsonContent))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                            throw new UnauthorizedAccessException("Você não está autenticado, deseja autenticar-se?");

                        throw new InvalidOperationException("Algo de errado não de deu certo ao consultar o CEP");
                    }

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado não de deu certo ao consultar o CEP");

                    return JsonConvert.DeserializeObject<CepDto>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class TokenResult
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string userName { get; set; }
            public string issued { get; set; }
            public string expires { get; set; }
        }

    }
}
